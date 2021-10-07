using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{


    public enum PieceType
    {
        Normal,
        Count,
        bubble,
        EMPTY,
        NORMAL,
    }
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    [System.Serializable]
    public struct PiecePosition
    {
        public PieceType type;
        public int x;
        public int y;
    };

    public int xDim;
    public int yDim;
    public float fillTime;

    // TODO Get automatically or at least make an assertion
    //  public Level level;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;



    private Dictionary<PieceType, GameObject> _piecePrefabDict;

    private GamePiece[,] _pieces;

    private bool _inverse = false;

    private GamePiece _pressedPiece;
    private GamePiece _enteredPiece;

    private bool _gameOver;

    private bool _isFilling;

    public bool IsFilling => _isFilling;

    private void Start()
    {
        // populating dictionary with piece prefabs types
        _piecePrefabDict = new Dictionary<PieceType, GameObject>();
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!_piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                _piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        
        // instantiate backgrounds
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                GameObject background = Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }


        _pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY);

            }
        }

        StartCoroutine(Fill());
    }
   
    private IEnumerator Fill()
    {


        while (FillStep())
        {
           
        }





    }

    /// <summary>
    /// One pass through all grid cells, moving them down one grid, if possible.
    /// </summary>
    /// <returns> returns true if at least one piece is moved down</returns>
   private bool FillStep()
    {
        bool movedPiece = false;
        // y = 0 is at the top, we ignore the last row, since it can't be moved down.
        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int x= 0; x < xDim;x++)
            {
                //     int x = loopX;
               /* if (_inverse)
                {
                    x = xDim - 1 - loopX;
                }*/
                GamePiece piece = _pieces[x, y];

                if (piece.IsMovable())
                {

                    GamePiece pieceBelow = _pieces[x, y + 1];

                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.Movable.Move(x, y + 1, fillTime);
                        _pieces[x, y + 1] = piece;

                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }

                }
            }

        }
     
   /*
                    else
                    {
                        for (int diag = -1; diag <= 1; diag++)
                        {
                            if (diag == 0)
                            {

                                int diagX = x + diag;

                                if (_inverse)
                                {
                                    diagX = x - diag;
                                }

                                if (diagX < 0 || diagX >= xDim)
                                {

                                    GamePiece diagonalPiece = _pieces[diagX, y + 1];

                                    if (diagonalPiece.Type != PieceType.EMPTY)
                                    {

                                        bool hasPieceAbove = true;

                                        for (int aboveY = y; aboveY >= 0; aboveY--)
                                        {
                                            GamePiece pieceAbove = _pieces[diagX, aboveY];

                                            if (pieceAbove.IsMovable())
                                            {
                                                break;
                                            }
                                            else if (!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
                                            {
                                                hasPieceAbove = false;
                                                break;
                                            }
                                        }

                                        if (hasPieceAbove)
                                        {

                                            Destroy(diagonalPiece.gameObject);
                                            piece.Movable.Move(diagX, y + 1, fillTime);
                                            _pieces[diagX, y + 1] = piece;
                                            SpawnNewPiece(x, y, PieceType.EMPTY);
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
   */
        // the highest row (0) is a special case, we must fill it with new pieces if empty
        for (int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = _pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY)
            {

                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(_piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;
                _pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                _pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                _pieces[x, 0].Movable.Move(x, 0, fillTime);

                _pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, _pieces[x, 0].ColorComponent.NumColors));
                movedPiece = true;
            }


        }
        return movedPiece;
    }
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x,
                           transform.position.y + yDim / 2.0f - y);
    }
    
    public GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {

        GameObject newPiece = (GameObject)Instantiate(_piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        _pieces[x, y] = newPiece.GetComponent<GamePiece>();
        _pieces[x, y].Init(x, y, this, type);

        return _pieces[x, y];
    }
    /*
    private static bool IsAdjacent(GamePiece piece1, GamePiece piece2)
    {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
            || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }
    
    private void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (piece1.IsMovable() && piece2.IsMovable())
        {
            _pieces[piece1.X, piece2.Y] = piece2;
            _pieces[piece2.X, piece2.Y] = piece1;

            int piece1x = piece1.X;
            int piece1y = piece1.Y;

            piece1.Movable.Move(piece2.X, piece2.Y, fillTime);
            piece2.Movable.Move(piece1.X, piece1.Y, fillTime);
        }



        else
        {
            _pieces[piece1.X, piece1.Y] = piece1;
            _pieces[piece2.X, piece2.Y] = piece2;
        }
    }

    public void PressPiece(GamePiece piece)
    {
        _pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece)
    {
        _enteredPiece = piece;
    }

    public void ReleasePiece()
    {
        if (IsAdjacent(_pressedPiece, _enteredPiece))
        {
            SwapPieces(_pressedPiece, _enteredPiece);
        }
    }


    */
}
