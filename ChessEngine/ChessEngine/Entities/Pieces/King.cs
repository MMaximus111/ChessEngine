using ChessEngine.Dictionaries;

namespace ChessEngine.Entities.Pieces;

public abstract class King : Piece
{
    public override byte Price { get; } = 100;

    public override IEnumerable<Move> GetValidMovements(Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.King[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.Piece == null || targetSquare.Piece.Color != Color)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, Icon, MovePriority.KingMove);
                    }
                }
            }
        }
    }
}