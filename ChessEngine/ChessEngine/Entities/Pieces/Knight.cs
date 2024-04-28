using ChessEngine.Dictionaries;

namespace ChessEngine.Entities.Pieces;

public abstract class Knight : Piece
{
    public override byte Price { get; } = 3;

    public override IEnumerable<Move> GetValidMovements(Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Knight[currentLocation];

        foreach (Location[] locationLine in locationLines)
        {
            foreach (Location newLocation in locationLine)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.Piece == null || targetSquare.Piece.Color != Color)
                {
                    if (!checkForCheck || !AnyChecks(currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, Icon, targetSquare.Piece == null ? MovePriority.Default : MovePriority.PieceCapture);
                    }
                }
            }
        }
    }
}