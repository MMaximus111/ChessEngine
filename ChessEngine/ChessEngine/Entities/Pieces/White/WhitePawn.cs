namespace ChessEngine.Entities.Pieces.White;

public sealed class WhitePawn : Pawn
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override char Icon { get; } = '♟';

    public override Piece DeepCopy()
    {
        return new WhitePawn();
    }
}