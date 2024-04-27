namespace ChessEngine.Entities.Pieces.White;

public sealed class WhiteKing : King
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override char Icon { get; } = '♚';
    public override Piece DeepCopy()
    {
        return new WhiteKing();
    }
}