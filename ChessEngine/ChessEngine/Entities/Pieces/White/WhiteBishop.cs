namespace ChessEngine.Entities.Pieces.White;

public sealed class WhiteBishop : Bishop
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override char Icon { get; } = '♝';

    public override Piece DeepCopy()
    {
        return new WhiteBishop();
    }
}