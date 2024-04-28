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

    public static IEnumerable<Move> GetValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        return pieceId switch
        {
            ChessDictionary.WhitePawnId or ChessDictionary.BlackPawnId => GetPawnValidMovements(pieceId, currentLocation, board, checkForCheck),
            ChessDictionary.WhiteKnightId or ChessDictionary.BlackKnightId => GetKnightValidMovements(pieceId, currentLocation, board, checkForCheck),
            ChessDictionary.WhiteBishopId or ChessDictionary.BlackBishopId => GetBishopValidMovements(pieceId, currentLocation, board, checkForCheck),
            ChessDictionary.WhiteRookId or ChessDictionary.BlackRookId => GetRookValidMovements(pieceId, currentLocation, board, checkForCheck),
            ChessDictionary.WhiteQueenId or ChessDictionary.BlackQueenId => GetQueenValidMovements(pieceId, currentLocation, board, checkForCheck),
            ChessDictionary.WhiteKingId or ChessDictionary.BlackKingId => GetKingValidMovements(pieceId, currentLocation, board, checkForCheck),
            _ => throw new ArgumentOutOfRangeException(nameof(pieceId), pieceId, null)
        };
    }
    
    private static IEnumerable<Move> GetBishopValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Bishop[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == null)
                {
                    if (!checkForCheck || !AnyChecks( pieceId, currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, MovePriority.Default);
                    }
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        if (!checkForCheck || !AnyChecks(pieceId, currentLocation, newLocation, board))
                        {
                            yield return new Move(currentLocation, newLocation, pieceId, MovePriority.PieceCapture);
                        }
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }
    
    private static IEnumerable<Move> GetQueenValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Queen[currentLocation];

        foreach (Location[] locationLine in locationLines)
        {
            foreach (Location location in locationLine)
            {
                Square targetSquare = board.GetSquare(location);

                if (targetSquare.PieceId == null)
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, location, board))
                    {
                        yield return new Move(currentLocation, location, pieceId, MovePriority.Default);
                    }
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        if (!checkForCheck || !AnyChecks(pieceId, currentLocation, location, board))
                        {
                            yield return new Move(currentLocation, location, pieceId, MovePriority.PieceCapture);
                        }
                    }
            
                    break;
                }
                
            }
        }
    }
    
    private static IEnumerable<Move> GetRookValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Rook[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == null)
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, MovePriority.Default);
                    }
                }
                else
                {
                    if (ChessDictionary.GetColorByPieceId(targetSquare.PieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                    {
                        if (!checkForCheck || !AnyChecks(pieceId, currentLocation, newLocation, board))
                        {
                            yield return new Move(currentLocation, newLocation, pieceId, MovePriority.PieceCapture);
                        }
                    }

                    // Stop if the path is blocked by a piece
                    break;
                }
            }
        }
    }
    
    public static IEnumerable<Move> GetKingValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.King[currentLocation];
        
        foreach (Location[] line in locationLines)
        {
            foreach (Location newLocation in line)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == null || ChessDictionary.GetColorByPieceId(targetSquare.PieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, MovePriority.KingMove);
                    }
                }
            }
        }
    }
    
    public static IEnumerable<Move> GetKnightValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        Location[][] locationLines = AllPossibleMoves.Knight[currentLocation];

        foreach (Location[] locationLine in locationLines)
        {
            foreach (Location newLocation in locationLine)
            {
                Square targetSquare = board.GetSquare(newLocation);

                if (targetSquare.PieceId == null || ChessDictionary.GetColorByPieceId(targetSquare.PieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, newLocation, board))
                    {
                        yield return new Move(currentLocation, newLocation, pieceId, targetSquare.PieceId == null ? MovePriority.Default : MovePriority.PieceCapture);
                    }
                }
            }
        }
    }
    
     public static IEnumerable<Move> GetPawnValidMovements(byte pieceId, Location currentLocation, Board board, bool checkForCheck)
    {
        // Determine the direction of movement based on the color of the pawn
        int direction = ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.White ? 1 : -1;
    
        // Check the square directly in front of the pawn
        Location frontLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + direction));
    
        if (Board.IsLocationOnBoard(frontLocation) && board.GetSquare(frontLocation).PieceId == null)
        {
            if (!checkForCheck || !AnyChecks(pieceId, currentLocation, frontLocation, board))
            {
                yield return new Move(currentLocation, frontLocation, pieceId, 0);
            }
    
            // Check if the pawn is on its initial position and the square two steps ahead is free
            if ((ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.White && currentLocation.Y == 2) || (ChessDictionary.GetColorByPieceId(pieceId) == PieceColor.Black && currentLocation.Y == 7))
            {
                Location twoStepsAheadLocation = new Location(currentLocation.X, (byte)(currentLocation.Y + 2 * direction));
    
                if (Board.IsLocationOnBoard(twoStepsAheadLocation) && board.GetSquare(twoStepsAheadLocation).PieceId == null)
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, twoStepsAheadLocation, board))
                    {
                        yield return new Move(currentLocation, frontLocation, pieceId, MovePriority.Default);
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
                byte? targetPieceId = board.GetSquare(diagonalLocation).PieceId;
    
                if (targetPieceId != null && ChessDictionary.GetColorByPieceId(targetPieceId.Value) != ChessDictionary.GetColorByPieceId(pieceId))
                {
                    if (!checkForCheck || !AnyChecks(pieceId, currentLocation, diagonalLocation, board))
                    {
                        yield return new Move(currentLocation, frontLocation, pieceId, MovePriority.PieceCapture);
                    }
                }
            }
        }
    }
     
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool AnyChecks(byte pieceId, Location currentLocation, Location possibleLocation, Board board)
    {
        Board testBoard = board.DeepCopy();
    
        testBoard.Move(new Move(currentLocation, possibleLocation, pieceId, 0));
    
        if (testBoard.IsKingInCheck(ChessDictionary.GetColorByPieceId(pieceId)))
        {
            return true;
        }
    
        return false;
    }
}