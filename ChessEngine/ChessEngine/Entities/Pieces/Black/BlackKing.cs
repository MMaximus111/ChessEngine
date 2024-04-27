namespace ChessEngine.Entities.Pieces.Black;

public sealed class BlackKing : King
{
    public override PieceColor Color { get; } = PieceColor.Black;

    public override string Icon { get; } = "♔";
    public override Piece DeepCopy()
    {
        return new BlackKing();
    }
}