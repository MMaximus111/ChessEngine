namespace ChessEngine.Entities.Pieces;

public abstract class Bishop : Piece
{
    private static readonly int[][] Directions = new int[][]
    {
        new int[] {1, 1}, new int[] {-1, -1}, new int[] {1, -1}, new int[] {-1, 1}
    };
    
    public override byte Price { get; } = 3;

    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int[] direction in Directions)
        {
            for (int i = 1; i < 8; i++)
            {
                byte newX = (byte)(currentLocation.X + direction[0] * i);
                byte newY = (byte)(currentLocation.Y + direction[1] * i);

                Location newLocation = new Location(newX, newY);
                
                if (Board.IsLocationOnBoard(newLocation))
                {
                    Square targetSquare = board.GetSquare(newLocation);

                    // Check if the target square is empty or contains an opponent's piece
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
                else
                {
                    // Stop if the location is outside the board
                    break;
                }
            }
        }
    }
}