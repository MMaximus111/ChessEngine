using ChessEngine.Dictionaries;

namespace ChessEngine.Entities.Pieces;

public abstract class Queen : Piece
{
    public override byte Price { get; } = 9;

    public override IEnumerable<Move> GetValidMovements(Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Queen[currentLocation];

        foreach (Location[] locationLine in locationLines)
        {
            foreach (Location location in locationLine)
            {
                Square targetSquare = board.GetSquare(location);

                if (targetSquare.Piece == null)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, location, board))
                    {
                        yield return new Move(currentLocation, location, Icon, MovePriority.Default);
                    }
                }
                else
                {
                    if (targetSquare.Piece.Color != Color)
                    {
                        if (!checkForCheck || !AnyChecks(currentLocation, location, board))
                        {
                            yield return new Move(currentLocation, location, Icon, MovePriority.PieceCapture);
                        }
                    }
            
                    break;
                }
                
            }
        }
    }
}