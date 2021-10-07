using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int x;
    private int y;

    public int X {
        get { return x; }

        set {
            if(IsMovable())
            { x = value; }
            
        }
      
    }
    public int Y
    {
        get { return y; }

        set
        {
            if (IsMovable())
            { y = value; }

        }
    }

    private Grid.PieceType type;
    public Grid.PieceType Type
    {
        get
        {
            return type;
        }
    }

    private Grid grid;
    public Grid GridRef
    {
        get
        {
            return grid;
        }
    }

    private MovePiece movable;

    public MovePiece Movable
    {
        get { return movable; }


    }

    private ColorPiece colorComponent;

    public ColorPiece ColorComponent
    {
        get { return colorComponent; }


    }
    private void Awake()
    {
        movable = GetComponent<MovePiece>();
        colorComponent = GetComponent<ColorPiece>();
    }
    

    public void Init(int dX,int dY,Grid dGrid, Grid.PieceType dType)
    {
        x = dX;
        y = dY;
        type = dType;
        grid = dGrid;
    }

    public bool IsMovable()
    {
        return movable != null;
    }

  public bool isColored()
    {
        return colorComponent != null;
    }
    
}
