namespace ChessEngine.Entities;

public readonly struct Location
{
    public Location(byte x, byte y)
    {
        X = x;
        Y = y;
    }

    public byte X { get; }

    public byte Y { get; }

    public override string ToString()
    {
        return $"{GetLetterFromX()}{Y}";
    }

    private string GetLetterFromX()
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