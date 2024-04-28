namespace ChessEngine.Entities;

public readonly struct Move
{
    public Move(Location from, Location to, char pieceIcon, MovePriority priority)
    {
        From = from;
        To = to;
        PieceIcon = pieceIcon;
        Priority = priority;
    }

    public Location From { get; }

    public Location To { get; }

    public char PieceIcon { get; }
    
    public MovePriority Priority { get; }

    public override string ToString()
    {
        return $"{PieceIcon} {To}";
    }
}