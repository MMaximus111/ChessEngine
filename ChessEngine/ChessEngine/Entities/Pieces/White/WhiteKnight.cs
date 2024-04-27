namespace ChessEngine.Entities.Pieces.White;

public sealed class WhiteKnight : Knight
{
    public override PieceColor Color { get; } = PieceColor.White;

    public override char Icon { get; } = '♞';

    public override Piece DeepCopy()
    {
        return new WhiteKnight();
    }
}