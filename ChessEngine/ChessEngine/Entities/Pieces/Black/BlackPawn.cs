namespace ChessEngine.Entities.Pieces.Black;

public sealed class BlackPawn : Pawn
{
    public override PieceColor Color { get; } = PieceColor.Black;

    public override string Icon { get; } = "♙";
    public override Piece DeepCopy()
    {
        return new BlackPawn();
    }
}