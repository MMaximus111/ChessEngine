namespace ChessEngine.Entities.Pieces.Black;

public class BlackKnight : Knight
{
    public override PieceColor Color { get; } = PieceColor.Black;

    public override string Icon { get; } = "♘";
    public override Piece DeepCopy()
    {
        return new BlackKnight();
    }
}