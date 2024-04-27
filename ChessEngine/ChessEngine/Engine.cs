using ChessEngine.Entities;

namespace ChessEngine;

public class Engine
{
    public decimal Evaluate(Board board)
    {
        if (board.CurrentMoveColor == PieceColor.Black)
        {
            if (board.IsKingInCheckmate(board.CurrentMoveColor))
            {
                return 300;
            }
        }

        if (board.CurrentMoveColor == PieceColor.White)
        {
            if (board.IsKingInCheckmate(board.CurrentMoveColor))
            {
                return -300;
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
            Parallel.ForEach(board.GetAllPossibleMoves(), new ParallelOptions() { MaxDegreeOfParallelism = allPossibleMoves.Length }, (move) =>
            {
                Board boardCopy = board.DeepCopy();

                boardCopy.Move(move);

                decimal score = Minimax(boardCopy, depth - 1, decimal.MinValue, decimal.MaxValue, false);

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

    private decimal Minimax(Board board, int depth, decimal alpha, decimal beta, bool isMaximizingPlayer)
    {
        if (depth == 0 || board.IsGameOver())
        {
            return Evaluate(board);
        }

        decimal value;
       object lockObject = new object();

       // var allPossibleMoves = board.GetAllPossibleMoves().ToArray();
       
        if (isMaximizingPlayer)
        {
            value = decimal.MinValue;

            // foreach (Move possibleMove in allPossibleMoves)
            // {
            //     Board boardCopy = board.DeepCopy();
            //     boardCopy.Move(possibleMove);
            //
            //     decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, false);
            //
            //     value = Math.Max(value, eval);
            //     alpha = Math.Max(alpha, eval);
            //
            //     if (beta <= alpha)
            //     {
            //         break;
            //     }
            // }
            
            Parallel.ForEach(board.GetAllPossibleMoves(), (move, parallelLoopState) =>
            {
                Board boardCopy = board.DeepCopy();
                boardCopy.Move(move);
            
                decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, false);
            
                lock (lockObject)
                {
                    value = Math.Max(value, eval);
                    alpha = Math.Max(alpha, eval);
                }
            
                if (beta <= alpha)
                {
                    parallelLoopState.Break();
                }
            });

            return value;
        }

        value = decimal.MaxValue;

        // foreach (Move possibleMove in allPossibleMoves)
        // {
        //     Board boardCopy = board.DeepCopy();
        //     boardCopy.Move(possibleMove);
        //
        //     decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, true);
        //
        //     value = Math.Min(value, eval);
        //     beta = Math.Min(beta, eval);
        //
        //     if (beta <= alpha)
        //     {
        //         break;
        //     }
        // }
        
        Parallel.ForEach(board.GetAllPossibleMoves(), (move, parallelLoopState) =>
        {
            Board boardCopy = board.DeepCopy();
            boardCopy.Move(move);
        
            decimal eval = Minimax(boardCopy, depth - 1, alpha, beta, true);
        
            lock (lockObject)
            {
                value = Math.Min(value, eval);
                beta = Math.Min(beta, eval);
            }
        
            if (beta <= alpha)
            {
                parallelLoopState.Break();
            }
        });

        return value;
    }
}