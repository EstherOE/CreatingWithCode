using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid : MonoBehaviour
{


    public enum PieceType
    {
        Normal,
        Count,
        bubble,
        Empty,
    };

    public int xDim;
    public int yDim;

    [System.Serializable]
    public struct piecePrefab
    {
        public PieceType type;
        public GameObject prefab;

    };
    private Dictionary<PieceType, GameObject> prefabDic;
    public GameObject backgroundPrefab;

    public piecePrefab[] prefab;
    private GamePiece[,] piece;

    private bool inverse = false;
    private GamePiece pressedPiece;
    private GamePiece enteredPiece;
    // Start is called before the first frame update



    void Start()
    {

        prefabDic = new Dictionary<PieceType, GameObject>();
        for (int i = 0; i < prefab.Length; i++)
        {
            if (!prefabDic.ContainsKey(prefab[i].type))
            {
                prefabDic.Add(prefab[i].type, prefab[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.name = "Grid (" + x + ", " + y + ")";
                background.transform.parent = transform;
            }
        }
        piece = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < xDim; y++)
            {

                SpawnNewPiece(x, y, PieceType.Empty);
            }
        }


        int random = Random.Range(0, 8);
        Destroy(piece[random, 4].gameObject);

        SpawnNewPiece(random, 4, PieceType.bubble);

        int randomx = Random.Range(0, 8);
        Destroy(piece[randomx, 4].gameObject);

        SpawnNewPiece(randomx, 4, PieceType.bubble);
        StartCoroutine(Fill());
    }








    public float fillTime;

    public IEnumerator Fill()
    {
        while (FillStep())
        {
            inverse = !inverse;
            yield return new WaitForSeconds(fillTime);
        }
    }


    public bool IsAdjacent(GamePiece obj1, GamePiece obj2)
    {

        return (obj1.X == obj2.X && (int)Mathf.Abs(obj1.Y - obj2.Y) == 1
            || obj1.Y == obj2.Y && (int)Mathf.Abs(obj1.X - obj2.X) == 1
            );
    }


    public void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (piece1.isMovable() && piece2.isMovable())
        {
            piece[piece1.X, piece2.Y] = piece2;
            piece[piece2.X, piece2.Y] = piece1;

            int piece1x = piece1.X;
            int piece1y = piece1.Y;

            piece1.Movable.Move(piece2.X, piece2.Y, fillTime);
            piece2.Movable.Move(piece1.X, piece1.Y, fillTime);
        }
    }


    public void PressePiece(GamePiece piece)
    {
        pressedPiece = piece;
    }

    public void EnteredPiece(GamePiece ePiece)
    {
        enteredPiece = ePiece;
    }

    public void Release()
    {
        if (IsAdjacent(pressedPiece, enteredPiece))
        {
            SwapPieces(pressedPiece, enteredPiece);
        }
    }
    public bool FillStep()
    {
        bool movedPiece = false;



        for (int y = yDim - 2; y >= 0; y--)
        {

            for (int loopx = 0; loopx < xDim; loopx++)
            {

                int x = loopx;
                if (inverse)
                {
                    x = xDim - 1 - loopx;
                }
                GamePiece objpiece = piece[x, y];
                if (objpiece.isMovable())
                {

                    GamePiece pieceBelow = piece[x, y + 1];
                    if (pieceBelow.Type == PieceType.Empty)
                    {
                        Destroy(pieceBelow.gameObject);
                        objpiece.Movable.Move(x, y + 1, fillTime);
                        piece[x, y + 1] = objpiece;

                        SpawnNewPiece(x, y, PieceType.Empty);
                        movedPiece = true;
                    }
                    else
                    {
                        for (int diag = -1; diag <= 1; diag++)

                        {
                            if (diag != 0)
                            {
                                int diagx = x + diag;
                                if (inverse)
                                {
                                    diagx = x - diag;
                                }
                                if (diagx >= 0 && diagx < xDim)
                                {
                                    GamePiece diagongalPiec = piece[diagx, y + 1];
                                    if (diagongalPiec.Type == PieceType.Empty)
                                    {
                                        bool hasPieceAbove = true;
                                        for (int aboveY = y; aboveY >= 0; aboveY--)
                                        {
                                            GamePiece pieceAbove = piece[diagx, aboveY];
                                            if (pieceAbove.isMovable())
                                            {
                                                break;
                                            }
                                            else if (!pieceAbove.isMovable() && pieceAbove.Type != PieceType.Empty)
                                            {
                                                hasPieceAbove = false;
                                                break;
                                            }
                                        }

                                        if (!hasPieceAbove)
                                        {
                                            Destroy(diagongalPiec.gameObject);
                                            objpiece.Movable.Move(diagx, y + 1, fillTime);
                                            piece[diagx, y + 1] = objpiece;
                                            SpawnNewPiece(x, y, PieceType.Empty);
                                            movedPiece = true;
                                            break;
                                        }
                                    }


                                }
                            }

                        }
                    }

                }

            }
        }
        for (int x = 0; x < xDim; x++)
        {
            int y = -1;

            GamePiece pieceBelow = piece[x, 0];
            if (pieceBelow.Type == PieceType.Empty)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(prefabDic[PieceType.Normal], GetWorldPosition(x, y), Quaternion.identity);
                newPiece.transform.parent = transform;

                piece[x, 0] = newPiece.GetComponent<GamePiece>();
                piece[x, 0].Init(x, -1, this, PieceType.Normal);
                piece[x, 0].Movable.Move(x, 0, fillTime);
                piece[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, piece[x, 0].ColorComponent.NumColors));
                movedPiece = true;
            }
        }

        return movedPiece;
    }

    public Vector2 GetWorldPosition(int x, int y)
    {

        return new Vector2(transform.position.x - xDim / 2 + x, transform.position.y + yDim / 2 - y);
    }



    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {

        GameObject newPiece = (GameObject)Instantiate(prefabDic[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        piece[x, y] = newPiece.GetComponent<GamePiece>();
        piece[x, y].Init(x, y, this, type);

        return piece[x, y];
    }


}

