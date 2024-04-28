using System.Runtime.CompilerServices;
using ChessEngine.Entities;
using ChessEngine.Helpers;

namespace ChessEngine;

public class Engine
{
    private long _evaluationCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Evaluate(Board board)
    {
        Interlocked.Increment(ref _evaluationCount);
        
        if (board.CurrentMoveColor == PieceColor.Black)
        {
            if (board.IsKingInCheckmate(board.CurrentMoveColor))
            {
                return 300000;
            }
        }

        if (board.CurrentMoveColor == PieceColor.White)
        {
            if (board.IsKingInCheckmate(board.CurrentMoveColor))
            {
                return -300000;
            }
        }
        
        return board.GetPiecesPrice(PieceColor.White) - board.GetPiecesPrice(PieceColor.Black);
    }

    public Move MakeTheBestMove(Board board, int depth)
    {
        Move? bestMove = null;
        Move? firstValidMove = null;
        decimal bestScore = 0;
        object lockObject = new object();
        
        Move[] allPossibleMoves = board.GetAllPossibleMoves().ToArray();
        
        if (allPossibleMoves.Any())
        {
            Parallel.ForEach(allPossibleMoves, new ParallelOptions() { MaxDegreeOfParallelism = allPossibleMoves.Length }, (move) =>
            {
                Board boardCopy = board.DeepCopy();
        
                boardCopy.Move(move);
        
                decimal score = Minimax(boardCopy, depth - 1, decimal.MinValue, decimal.MaxValue,  false);
        
                lock (lockObject)
                {
                    firstValidMove ??= move;
                
                    if (board.CurrentMoveColor == PieceColor.Black)
                    {
                        if (score <= bestScore)
                        {
                            bestScore = score;
                            bestMove = move;
                        }
                    }
                
                    if (board.CurrentMoveColor == PieceColor.White)
                    {
                        if (score >= bestScore)
                        {
                            bestScore = score;
                            bestMove = move;
                        }
                    }
                }
            });
        }

        if (bestMove == null && firstValidMove == null)
        {
            if (board.IsKingInCheck(board.CurrentMoveColor))
            {
                throw new InvalidOperationException("Checkmate!");
            }

            throw new InvalidOperationException("Stalemate!");
        }

        Move finalMove = bestMove ?? firstValidMove!.Value;
        
        board.Move(finalMove);

        return finalMove;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private decimal Minimax(Board board, int depth, decimal alpha, decimal beta, bool isMaximizingPlayer)
    {
        if (depth == 0)
        {
            return Evaluate(board);
        }

        decimal value;
        // object lockObject = new object();

       IEnumerable<Move> allPossibleMoves = board.GetAllPossibleMoves().OrderByPriorityDesc();
       
        if (isMaximizingPlayer)
        {
            value = decimal.MinValue;

            foreach (Move possibleMove in allPossibleMoves)
            {
                Board boardCopy = board.DeepCopy();
            
                boardCopy.Move(possibleMove);
            
                decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, false);
            
                value = Math.Max(value, eval);
                alpha = Math.Max(alpha, eval);
            
                if (beta <= alpha)
                {
                    break;
                }
            }

            return value;
        }

        value = decimal.MaxValue;

        foreach (Move possibleMove in allPossibleMoves)
        {
            Board boardCopy = board.DeepCopy();
        
            boardCopy.Move(possibleMove);
        
            decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, true);
        
            value = Math.Min(value, eval);
        
            beta = Math.Min(beta, eval);
        
            if (beta <= alpha)
            {
                break;
            }
        }

        return value;
    }

    public long GetEvaluationsCount() => _evaluationCount;
}