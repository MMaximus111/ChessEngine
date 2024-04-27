using ChessEngine.Entities.Pieces;

namespace ChessEngine.Entities;

public sealed class Square
{
    public Square(Location location, Piece? piece)
    {
        Location = location;
        Piece = piece;
    }

    public Location Location { get; }
    
    public Piece? Piece { get; private set; }
    
    public void Clear()
    {
        Piece = null;
    }
    
    public void SetPiece(Piece piece)
    {
        Piece = piece;
    }
}