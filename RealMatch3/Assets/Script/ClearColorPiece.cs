public class ClearColorPiece : ClearablePiece
{
    private ColorPiece.ColorType _color;

    public ColorPiece.ColorType Color
    {
        get => _color;
        set => _color = value;
    }

    public override void Clear()
    {
        base.Clear();

        piece.GridRef.ClearColor(_color);
    }
}
