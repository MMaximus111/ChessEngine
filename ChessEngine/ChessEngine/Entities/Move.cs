namespace ChessEngine.Entities;

public readonly struct Move
{
    public Move(Location from, Location to, char pieceIcon)
    {
        From = from;
        To = to;
        PieceIcon = pieceIcon;
    }

    public Location From { get; }

    public Location To { get; }

    public char PieceIcon { get; }

    public override string ToString()
    {
        return $"{PieceIcon} {To}";
    }
}