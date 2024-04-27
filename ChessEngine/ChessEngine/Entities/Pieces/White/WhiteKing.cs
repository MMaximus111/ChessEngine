namespace ChessEngine.Entities.Pieces.White;

public class WhiteKing : King
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override string Icon { get; } = "♚";
    public override Piece DeepCopy()
    {
        return new WhiteKing();
    }
}