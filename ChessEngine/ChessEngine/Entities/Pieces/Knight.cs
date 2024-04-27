using ChessEngine.Helpers;

namespace ChessEngine.Entities.Pieces;

public abstract class Knight : Piece
{
    public override byte Price { get; } = 3;
    
    private static readonly int[] Offsets = [-2, -1, 1, 2];
    
    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int x in Offsets)
        {
            foreach (int y in Offsets)
            {
                if (FastMath.Abs(x) == FastMath.Abs(y))
                {
                    // Skip if the move is not in an "L" shape
                    continue;
                }

                byte newX = (byte)(currentLocation.X + x);
                byte newY = (byte)(currentLocation.Y + y);

                if (newX > 0 && newX <= 8 && newY > 0 && newY <= 8)
                {
                    Location newLocation = new Location(newX, newY);

                    Square targetSquare = board.GetSquare(newLocation);

                    // Check if the target square is empty or contains an opponent's piece
                    if (targetSquare.Piece == null || targetSquare.Piece.Color != Color)
                    {
                        if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                        {
                            yield return newLocation;
                        }
                    }
                }
            }
        }
    }
}