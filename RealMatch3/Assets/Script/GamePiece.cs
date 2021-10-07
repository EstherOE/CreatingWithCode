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
            if(isMovable())
            { x = value; }
            
        }
      
    }
    public int Y
    {
        get { return y; }

        set
        {
            if (isMovable())
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

    private ClearablePiece _clearableComponent;

    public ClearablePiece ClearableComponent => _clearableComponent;
    private void Awake()
    {
        movable = GetComponent<MovePiece>();
        colorComponent = GetComponent<ColorPiece>();
        _clearableComponent = GetComponent<ClearablePiece>();
    }
    

    public void Init(int dX,int dY,Grid dGrid, Grid.PieceType dType)
    {
        x = dX;
        y = dY;
        type = dType;
        grid = dGrid;
    }

    public bool isMovable()
    {
        return movable != null;
    }

  public bool isColored()
    {
        return colorComponent != null;
    }

    public bool IsClearable()
    {
        return _clearableComponent != null;
    }

    private void OnMouseEnter()
    {
        grid.EnterPiece(this);
    }

    private void OnMouseDown()
    {
        grid.PressPiece(this);
    }

    private void OnMouseUp()
    {
        grid.ReleasePiece();
    }

}
