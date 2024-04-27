namespace ChessEngine.Entities.Pieces.Black;

public sealed class BlackRook : Rook
{
    public override PieceColor Color { get; } = PieceColor.Black;

    public override char Icon { get; } = '♖';

    public override Piece DeepCopy()
    {
        return new BlackRook();
    }
}