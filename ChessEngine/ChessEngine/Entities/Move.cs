namespace ChessEngine.Entities;

public readonly struct Move
{
    public Move(Location from, Location to, byte pieceId, MovePriority priority)
    {
        From = from;
        To = to;
        PieceId = pieceId;
        Priority = priority;
    }

    public Location From { get; }

    public Location To { get; }

    public byte PieceId { get; }
    
    public MovePriority Priority { get; }

    public override string ToString()
    {
        return $"{PieceId} {To}";
    }
}