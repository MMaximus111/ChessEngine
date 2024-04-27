namespace ChessEngine.Entities.Pieces.White;

public sealed class WhiteBishop : Bishop
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override string Icon { get; } = "♝";
    public override Piece DeepCopy()
    {
        return new WhiteBishop();
    }
}