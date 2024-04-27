namespace ChessEngine.Entities;

public readonly struct Location
{
    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }

    public int Y { get; init; }

    public override string ToString()
    {
        return $"{GetLetterFromX()}{Y}";
    }

    public string GetLetterFromX()
    {
        return X switch
        {
            1 => "a",
            2 => "b",
            3 => "c",
            4 => "d",
            5 => "e",
            6 => "f",
            7 => "g",
            8 => "h",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}