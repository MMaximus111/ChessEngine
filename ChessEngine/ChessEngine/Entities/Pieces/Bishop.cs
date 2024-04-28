using ChessEngine.Dictionaries;

namespace ChessEngine.Entities.Pieces;

public abstract class Bishop : Piece
{
    public override byte Price { get; } = 3;

    public override IEnumerable<Move> GetValidMovements(Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Bishop[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.Piece == null)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, Icon, MovePriority.Default);
                    }
                }
                else
                {
                    if (targetSquare.Piece.Color != Color)
                    {
                        if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                        {
                            yield return new Move(currentLocation, newLocation, Icon, MovePriority.PieceCapture);
                        }
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }
}