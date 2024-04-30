﻿using System.Net;
using System.Runtime.CompilerServices;
using ChessEngine.Entities;
using ChessEngine.Entities.Pieces;

namespace ChessEngine;

public class Engine
{
    private long _evaluationCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Evaluate(Board board)
    {
        _evaluationCount++;
        
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

        Move[] allPossibleMoves = board.GetAllPossibleMoves().ToArray();

        if (allPossibleMoves.Any())
        {
            foreach (Move move in allPossibleMoves)
            {
                Board boardCopy = board.DeepCopy();

                (bool ValidMove, byte? capturedPieceId) result = boardCopy.Move(move);

                if (!result.ValidMove)
                {
                    continue;
                }

                decimal score = Minimax(boardCopy, depth - 1, decimal.MinValue, decimal.MaxValue, true);

                firstValidMove ??= move;

                if (board.CurrentMoveColor == PieceColor.Black && score <= bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                if (board.CurrentMoveColor == PieceColor.White && score >= bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }
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

       IEnumerable<Move> allPossibleMoves = board.GetAllPossibleMoves();
       
        if (isMaximizingPlayer)
        {
            value = decimal.MinValue;

            foreach (Move possibleMove in allPossibleMoves)
            {
                (bool ValidMove, byte? capturedPieceId) result = board.Move(possibleMove);
                
                if (!result.ValidMove)
                {
                    continue;
                }
                
                decimal eval = Minimax(board, depth - 1, alpha, beta, false);
            
                value = Math.Max(value, eval);
                alpha = Math.Max(alpha, eval);
            
                board.UndoMove(possibleMove, result.capturedPieceId);
                
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
            (bool ValidMove, byte? capturedPieceId) result = board.Move(possibleMove);
        
            if (!result.ValidMove)
            {
                continue;
            }

            decimal eval = Minimax(board, depth - 1, alpha, beta, true);
        
            value = Math.Min(value, eval);
            beta = Math.Min(beta, eval);
        
            board.UndoMove(possibleMove, result.capturedPieceId);
            
            if (beta <= alpha)
            {
                break;
            }
        }
        
        return value;
    }

    public long GetEvaluationsCount() => _evaluationCount;
}