using System.Runtime.CompilerServices;

namespace ChessEngine.Entities.Pieces;

public abstract class Piece
{
    public abstract PieceColor Color { get; }
    
    public abstract byte Price { get; }
    
    public abstract string Name { get; }
    
    public abstract string Icon { get; }
    
    public abstract IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck);

    public abstract Piece DeepCopy();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool AnyChecks(Location currentLocation, Location possibleLocation, Board board)
    {
        Board testBoard = board.DeepCopy();

        testBoard.Move(new Move(currentLocation, possibleLocation, this));

        if (testBoard.IsKingInCheck(Color))
        {
            return true;
        }

        return false;
    }
}