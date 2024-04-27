using ChessEngine.Entities.Pieces;

namespace ChessEngine.Entities;

public class Square
{
    public Square(Location location, Piece? piece)
    {
        Location = location;
        Piece = piece;
    }

    public Location Location { get; }
    
    public Piece? Piece { get; set; }
    
    public void Clear()
    {
        Piece = null;
    }
    
    public void SetPiece(Piece piece)
    {
        Piece = piece;
    }
}