namespace ChessEngine.Entities.Pieces;

public abstract class Pawn : Piece
{
    public override byte Price { get; } = 1;
    
    public override IEnumerable<Location> GetValidLocationsToMove(Location currentLocation, Board board, bool checkForCheck)
    {
        // Determine the direction of movement based on the color of the pawn
        int direction = Color == PieceColor.White ? 1 : -1;

        // Check the square directly in front of the pawn
        Location frontLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + direction));

        if (Board.IsLocationOnBoard(frontLocation) && board.GetSquare(frontLocation).Piece == null)
        {
            if (!checkForCheck || !AnyChecks(currentLocation, frontLocation, board))
            {
                yield return frontLocation;
            }

            // Check if the pawn is on its initial position and the square two steps ahead is free
            if ((Color == PieceColor.White && currentLocation.Y == 2) || (Color == PieceColor.Black && currentLocation.Y == 7))
            {
                Location twoStepsAheadLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + 2 * direction));

                if (Board.IsLocationOnBoard(twoStepsAheadLocation) && board.GetSquare(twoStepsAheadLocation).Piece == null)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, twoStepsAheadLocation, board))
                    {
                        yield return twoStepsAheadLocation;
                    }
                }
            }
        }

        // Check the squares diagonally in front of the pawn for opponent's pieces
        Location[] diagonalLocations = new Location[]
        {
            new Location((byte)(currentLocation.X - 1), (byte)(currentLocation.Y + direction)),
            new Location((byte)(currentLocation.X + 1), (byte)(currentLocation.Y + direction))
        };

        foreach (Location diagonalLocation in diagonalLocations)
        {
            if (Board.IsLocationOnBoard(diagonalLocation))
            {
                Piece? targetPiece = board.GetSquare(diagonalLocation).Piece;

                if (targetPiece != null && targetPiece.Color != this.Color)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, diagonalLocation, board))
                    {
                        yield return diagonalLocation;
                    }
                }
            }
        }
    }
}