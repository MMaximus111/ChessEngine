namespace ChessEngine.Entities.Pieces;

public abstract class Queen : Piece
{
    private static readonly int[][] Directions = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 },
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };

    public override byte Price { get; } = 9;

    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int[] direction in Directions)
        {
            for (byte i = 1; i < 8; i++)
            {
                Location newLocation = new Location((byte)(currentLocation.X + direction[0] * i), (byte)(currentLocation.Y + direction[1] * i));
                
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