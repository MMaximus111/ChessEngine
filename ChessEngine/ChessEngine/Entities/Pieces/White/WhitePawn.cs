namespace ChessEngine.Entities.Pieces.White;

public class WhitePawn : Pawn
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override string Icon { get; } = "♟";
    public override Piece DeepCopy()
    {
        return new WhitePawn();
    }
}