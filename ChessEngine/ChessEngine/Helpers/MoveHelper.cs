using System.Runtime.CompilerServices;
using ChessEngine.Dictionaries;
using ChessEngine.Entities;
using ChessEngine.Entities.Pieces;

namespace ChessEngine.Helpers;

public static class MoveHelper
{
    public static IEnumerable<Move> OrderByPriorityDesc(this IEnumerable<Move> moves)
    {
        return moves.OrderByDescending(x => x.Priority);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<Move> GetValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        return pieceId switch
        {
            ChessDictionary.WhitePawnId or ChessDictionary.BlackPawnId => GetPawnValidMovements(pieceId, currentLocation, board),
            ChessDictionary.WhiteKnightId or ChessDictionary.BlackKnightId => GetKnightValidMovements(pieceId, currentLocation, board),
            ChessDictionary.WhiteBishopId or ChessDictionary.BlackBishopId => GetBishopValidMovements(pieceId, currentLocation, board),
            ChessDictionary.WhiteRookId or ChessDictionary.BlackRookId => GetRookValidMovements(pieceId, currentLocation, board),
            ChessDictionary.WhiteQueenId or ChessDictionary.BlackQueenId => GetQueenValidMovements(pieceId, currentLocation, board),
            ChessDictionary.WhiteKingId or ChessDictionary.BlackKingId => GetKingValidMovements(pieceId, currentLocation, board),
            _ => throw new ArgumentOutOfRangeException(nameof(pieceId), pieceId, null)
        };
    }

    private static IEnumerable<Move> GetBishopValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        foreach (Location[] line in AllPossibleMoves.Bishop[currentLocation])
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == ChessDictionary.NoneId)
                {
                    yield return new Move(currentLocation, newLocation, pieceId, MovePriority.Default);
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, MovePriority.PieceCapture);
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }

    private static IEnumerable<Move> GetQueenValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        foreach (Location[] locationLine in AllPossibleMoves.Queen[currentLocation])
        {
            foreach (Location location in locationLine)
            {
                Square targetSquare = board.GetSquare(location);

                if (targetSquare.PieceId == ChessDictionary.NoneId)
                {
                    yield return new Move(currentLocation, location, pieceId, MovePriority.Default);
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        yield return new Move(currentLocation, location, pieceId, MovePriority.PieceCapture);
                    }

                    break;
                }
            }
        }
    }

    private static IEnumerable<Move> GetRookValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        foreach (Location[] line in AllPossibleMoves.Rook[currentLocation])
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == ChessDictionary.NoneId)
                {
                    yield return new Move(currentLocation, newLocation, pieceId, MovePriority.Default);
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, MovePriority.PieceCapture);
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }

    private static IEnumerable<Move> GetKingValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        foreach (Location[] line in AllPossibleMoves.King[currentLocation])
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == ChessDictionary.NoneId || ChessDictionary.GetColorByPieceId(targetSquare.PieceId) != ChessDictionary.GetColorByPieceId(pieceId))
                {
                    yield return new Move(currentLocation, newLocation, pieceId, MovePriority.KingMove);
                }
            }
        }
    }

    private static IEnumerable<Move> GetKnightValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        foreach (Location[] locationLine in AllPossibleMoves.Knight[currentLocation])
        {
            foreach (Location newLocation in locationLine)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == ChessDictionary.NoneId || ChessDictionary.GetColorByPieceId(targetSquare.PieceId) != ChessDictionary.GetColorByPieceId(pieceId))
                {
                    yield return new Move(currentLocation, newLocation, pieceId, targetSquare.PieceId == ChessDictionary.NoneId ? MovePriority.Default : MovePriority.PieceCapture);
                }
            }
        }
    }

    private static IEnumerable<Move> GetPawnValidMovements(byte pieceId, Location currentLocation, Board board)
    {
        // Determine the direction of movement based on the color of the pawn
        int direction = ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.White ? 1 : -1;

        // Check the square directly in front of the pawn
        Location frontLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + direction));

        if (Board.IsLocationOnBoard(frontLocation) && board.GetSquare(frontLocation).PieceId == ChessDictionary.NoneId)
        {
            yield return new Move(currentLocation, frontLocation, pieceId, 0);

            // Check if the pawn is on its initial position and the square two steps ahead is free
            if ((ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.White && currentLocation.Y == 2) || (ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.Black && currentLocation.Y == 7))
            {
                Location twoStepsAheadLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + 2 * direction));

                if (Board.IsLocationOnBoard(twoStepsAheadLocation) && board.GetSquare(twoStepsAheadLocation).PieceId == ChessDictionary.NoneId)
                {
                    yield return new Move(currentLocation, frontLocation, pieceId, MovePriority.Default);
                }
            }
        }

        Location leftDiagonalLocation = new Location((byte)(currentLocation.X - 1), (byte)(currentLocation.Y + direction));
        Location rightDiagonalLocation = new Location((byte)(currentLocation.X + 1), (byte)(currentLocation.Y + direction));
        
        if (Board.IsLocationOnBoard(leftDiagonalLocation))
        {
            byte? targetPieceId = board.GetSquare(leftDiagonalLocation).PieceId;

            if (targetPieceId != ChessDictionary.NoneId && ChessDictionary.GetColorByPieceId(targetPieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
            {
                yield return new Move(currentLocation, frontLocation, pieceId, MovePriority.PieceCapture);
            }
        }
        
        if (Board.IsLocationOnBoard(rightDiagonalLocation))
        {
            byte? targetPieceId = board.GetSquare(rightDiagonalLocation).PieceId;

            if (targetPieceId != ChessDictionary.NoneId && ChessDictionary.GetColorByPieceId(targetPieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
            {
                yield return new Move(currentLocation, frontLocation, pieceId, MovePriority.PieceCapture);
            }
        }
    }
}