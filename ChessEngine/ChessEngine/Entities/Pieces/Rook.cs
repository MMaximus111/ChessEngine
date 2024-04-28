using ChessEngine.Dictionaries;

namespace ChessEngine.Entities.Pieces;

public abstract class Rook : Piece
{
    public override byte Price { get; } = 5;
    
    //private static readonly int[][] Directions = new int[][] { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };

    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Rook[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.Piece == null)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                    {
                        yield return newLocation;
                    }
                }
                else
                {
                    if (targetSquare.Piece.Color != Color)
                    {
                        if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                        {
                            yield return newLocation;
                        }
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }
}