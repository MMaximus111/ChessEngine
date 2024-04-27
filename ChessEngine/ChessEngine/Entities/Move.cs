using ChessEngine.Entities.Pieces;

namespace ChessEngine.Entities;

public readonly struct Move
{
    public Move(Location from, Location to, Piece capturedPiece)
    {
        From = from;
        To = to;
        CapturedPiece = capturedPiece;
    }

    public Location From { get; }

    public Location To { get; }

    public Piece CapturedPiece { get; }
    
    public Move DeepCopy()
    {
        return new Move(From, To, CapturedPiece.DeepCopy());
    }

    public override string ToString()
    {
        return $"{CapturedPiece.Icon} {To}";
    }
}