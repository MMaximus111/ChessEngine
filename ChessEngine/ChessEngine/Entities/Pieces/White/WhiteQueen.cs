namespace ChessEngine.Entities.Pieces.White;

public sealed class WhiteQueen : Queen
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override char Icon { get; } = '♛';

    public override Piece DeepCopy()
    {
        return new WhiteQueen();
    }
}