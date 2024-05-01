namespace ChessEngine.Entities;

public struct Square
{
    public Square(Location location, byte pieceId)
    {
        Location = location;
        PieceId = pieceId;
    }

    public Location Location { get; }
    
    public byte PieceId { get; private set; }
    
    public void Clear()
    {
        PieceId = default;
    }
    
    public void SetPiece(byte pieceId)
    {
        PieceId = pieceId;
    }
}