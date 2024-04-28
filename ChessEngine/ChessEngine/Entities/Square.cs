using ChessEngine.Entities.Pieces;

namespace ChessEngine.Entities;

public struct Square
{
    public Square(Location location, byte? pieceId)
    {
        Location = location;
        PieceId = pieceId;
    }

    public Location Location { get; }
    
    public byte? PieceId { get; private set; }
    
    public void Clear()
    {
        PieceId = null;
    }
    
    public void SetPiece(byte? pieceId)
    {
        PieceId = pieceId;
    }
}