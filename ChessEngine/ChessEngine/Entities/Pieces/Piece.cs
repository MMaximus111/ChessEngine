using System.Runtime.CompilerServices;

namespace ChessEngine.Entities.Pieces;

public abstract class Piece
{
    public abstract PieceColor Color { get; }
    
    public abstract byte Price { get; }
    
    public abstract char Icon { get; }
    
    public abstract IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck);

    public abstract Piece DeepCopy();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool AnyChecks(Location currentLocation, Location possibleLocation, Board board)
    {
        Board testBoard = board.DeepCopy();
    
        testBoard.Move(new Move(currentLocation, possibleLocation, Icon));
    
        if (testBoard.IsKingInCheck(Color))
        {
            return true;
        }
    
        return false;
    }
    
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // protected bool AnyChecks(Location currentLocation, Location possibleLocation, Board board)
    // {
    //     Move move = new Move(currentLocation, possibleLocation, Icon);
    //     
    //     board.Move(move);
    //
    //     if (board.IsKingInCheck(Color))
    //     {
    //         board.ReverseLastMove(move);
    //         return true;
    //     }
    //     
    //     board.ReverseLastMove(move);
    //
    //     return false;
    // }
}