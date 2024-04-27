namespace ChessEngine.Entities.Pieces;

public abstract class Rook : Piece
{
    public override byte Price { get; } = 5;

    public override string Name { get; } = "R";
    
    private static readonly int[][] Directions = new int[][] { new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { 0, 1 } };
    
    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        foreach (int[] direction in Directions)
        {
            for (byte i = 1; i <= 8; i++)
            {
                int newX = currentLocation.X + direction[0] * i;
                int newY = currentLocation.Y + direction[1] * i;

                Location newLocation = new Location(newX, newY);
                
                if (!Board.IsLocationOnBoard(newLocation))
                {
                    // Out of board bounds
                    break;
                }

                Square square = board.GetSquare(newLocation);
                Piece? piece = square.Piece;

                if (piece == null)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, square.Location, board))
                    {
                        yield return square.Location;
                    }
                }
                else if (piece.Color != Color)
                {
                    // Opponent's piece
                    if (!checkForCheck || !AnyChecks(currentLocation, square.Location, board))
                    {
                        yield return square.Location;
                    }
                    break;
                }
                else
                {
                    // Own piece
                    break;
                }
            }
        }
    }
}