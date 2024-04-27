namespace ChessEngine.Entities.Pieces;

public abstract class King : Piece
{
    public override byte Price { get; } = 100;

    private static readonly int[][] Directions = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 },
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };

    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int[] direction in Directions)
        {
            byte newX = (byte)(currentLocation.X + direction[0]);
            byte newY = (byte)(currentLocation.Y + direction[1]);

            Location newLocation = new Location(newX, newY);
            
            if (Board.IsLocationOnBoard(newLocation))
            {
                Square targetSquare = board.GetSquare(newLocation);

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