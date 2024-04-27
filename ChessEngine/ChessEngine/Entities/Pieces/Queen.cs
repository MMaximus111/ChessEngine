namespace ChessEngine.Entities.Pieces;

public abstract class Queen : Piece
{
    private static readonly int[][] Directions = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 },
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };

    public override byte Price { get; } = 9;

    public override string Name { get; } = "Q";

    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int[] direction in Directions)
        {
            for (byte i = 1; i < 8; i++)
            {
                int newX = currentLocation.X + direction[0] * i;
                int newY = currentLocation.Y + direction[1] * i;

                if (newX > 0 && newX <= 8 && newY > 0 && newY <= 8)
                {
                    Location newLocation = new Location(newX, newY);

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

                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}