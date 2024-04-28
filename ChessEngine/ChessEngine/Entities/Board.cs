using System.Runtime.CompilerServices;
using System.Text;
using ChessEngine.Dictionaries;
using ChessEngine.Entities.Pieces;
using ChessEngine.Helpers;

namespace ChessEngine.Entities;

public class Board
{
   public Board(bool empty = false)
   {
       Squares = new Square[8, 8];

       for (byte i = 0; i < 8; i++)
       {
           for (byte j = 0; j < 8; j++)
           {
               Location location = new Location((byte)(i + 1), (byte)(j + 1));

               byte? pieceId = null;

               if (!empty)
               {
                   pieceId = location switch
                   {
                       { X: 1, Y: 1 } => ChessDictionary.WhitePawnId,
                       { X: 2, Y: 1 } => ChessDictionary.WhiteKnightId,
                       { X: 3, Y: 1 } => ChessDictionary.WhiteBishopId,
                       { X: 4, Y: 1 } => ChessDictionary.WhiteQueenId,
                       { X: 5, Y: 1 } => ChessDictionary.WhiteKingId,
                       { X: 6, Y: 1 } => ChessDictionary.WhiteBishopId,
                       { X: 7, Y: 1 } => ChessDictionary.WhiteKnightId,
                       { X: 8, Y: 1 } => ChessDictionary.WhiteRookId,
                       { Y: 2 } => ChessDictionary.WhitePawnId,
                       { Y: 7 } => ChessDictionary.BlackPawnId,
                       { X: 1, Y: 8 } => ChessDictionary.BlackRookId,
                       { X: 2, Y: 8 } => ChessDictionary.BlackKnightId,
                       { X: 3, Y: 8 } => ChessDictionary.BlackBishopId,
                       { X: 4, Y: 8 } => ChessDictionary.BlackQueenId,
                       { X: 5, Y: 8 } => ChessDictionary.BlackKingId,
                       { X: 6, Y: 8 } => ChessDictionary.BlackBishopId,
                       { X: 7, Y: 8 } => ChessDictionary.BlackKnightId,
                       { X: 8, Y: 8 } => ChessDictionary.BlackRookId,
                       _ => null
                   };
                   
                   WhiteKingSquare = GetSquare(new Location(5, 1));
                   BlackKingSquare = GetSquare(new Location(5, 8));
               }

               Squares[i, j] = new Square(location, pieceId);
           }
       }
   }
   
   public Square? WhiteKingSquare { get; private set; }
   
   public Square? BlackKingSquare { get; private set; }
    
    public PieceColor CurrentMoveColor { get; private set; } = PieceColor.White;

    public byte WhitePiecesPrice { get; private set; } = 39;
    
    public byte BlackPiecesPrice { get; private set; } = 39;

    public Square[,] Squares { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKingInCheck(PieceColor kingColor)
    {
        Square? kingSquare = kingColor == PieceColor.White ? WhiteKingSquare : BlackKingSquare;

        if (kingSquare is null)
        {
            return true;
        }

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                Square square = Squares[i, j];
                byte? pieceId = square.PieceId;
        
                if (pieceId != null && ChessDictionary.GetColorByPieceId(pieceId.Value) != kingColor && pieceId != ChessDictionary.WhiteKingId && pieceId != ChessDictionary.BlackKingId)
                {
                    foreach (Move move in MoveHelper.GetValidMovements(pieceId.Value, square.Location, this, false))
                    {
                        if (move.To.Equals(kingSquare.Value.Location))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLocationOnBoard(Location location)
    {
        return location.X >= 1 && location.X <= 8 && location.Y >= 1 && location.Y <= 8;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Move> GetAllPossibleMoves()
    {
        foreach (Square square in Squares)
        {
            byte? pieceId = square.PieceId;

            if (pieceId != null && ChessDictionary.GetColorByPieceId(pieceId.Value) == CurrentMoveColor)
            {
                foreach (Move move in MoveHelper.GetValidMovements(pieceId.Value, square.Location, this, true))
                {
                    yield return move;
                }
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsGameOver()
    {
        return !GetAllPossibleMoves().Any();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsKingInCheckmate(PieceColor kingColor)
    {
        return IsKingInCheck(kingColor) && IsGameOver();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Move(Move move)
    {
        byte fromX = (byte)(move.From.X - 1);
        byte fromY = (byte)(move.From.Y - 1);
        byte toX = (byte)(move.To.X - 1);
        byte toY = (byte)(move.To.Y - 1);

        if (Squares[toX, toY].PieceId is not null)
        {
            if (CurrentMoveColor == PieceColor.White)
            {
                BlackPiecesPrice -= ChessDictionary.PiecePrices[Squares[toX, toY].PieceId!.Value];
            }
            else
            {
                WhitePiecesPrice -= ChessDictionary.PiecePrices[Squares[toX, toY].PieceId!.Value];
            }
        }

        Squares[toX, toY].SetPiece(Squares[fromX, fromY].PieceId);
        Squares[fromX, fromY].Clear();

        CurrentMoveColor = CurrentMoveColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        if (Squares[toX, toY].PieceId is ChessDictionary.WhiteKingId or ChessDictionary.BlackKingId)
        {
            if (Squares[toX, toY].PieceId == ChessDictionary.WhiteKingId)
            {
                WhiteKingSquare = Squares[toX, toY];
            }
            else
            {
                BlackKingSquare = Squares[toX, toY];
            }
        }
    }
    
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public void Move(Move move)
    // {
    //     Square fromSquare = GetSquare(move.From);
    //     Square toSquare = GetSquare(move.To);
    //
    //     if (toSquare.PieceId is not null)
    //     {
    //         if (CurrentMoveColor == PieceColor.White)
    //         {
    //             BlackPiecesPrice -= ChessDictionary.PiecePrices[toSquare.PieceId.Value];
    //         }
    //         else
    //         {
    //             WhitePiecesPrice -= ChessDictionary.PiecePrices[toSquare.PieceId.Value];
    //         }
    //     }
    //     
    //     toSquare.SetPiece(fromSquare.PieceId);
    //     fromSquare.Clear();
    //     
    //     CurrentMoveColor = CurrentMoveColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    //     
    //     if (toSquare.PieceId is ChessDictionary.WhiteKingId or ChessDictionary.BlackKingId)
    //     {
    //         if (toSquare.PieceId == ChessDictionary.WhiteKingId)
    //         {
    //             WhiteKingSquare = toSquare;
    //         }
    //         else
    //         {
    //             BlackKingSquare = toSquare;
    //         }
    //     }
    // }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal GetPiecesPrice(PieceColor color)
    {
        return color == PieceColor.White ? WhitePiecesPrice : BlackPiecesPrice;
    }
    
    public override string ToString()
    {
        const string emptyLightSquareSymbol = "  ";
        const string emptyDarkSquareSymbol = "\u2591\u2591";
        const char verticalSeparator = '|';
    
        StringBuilder boardString = new StringBuilder();
    
        for (int i = 7; i >= 0; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                byte? pieceId = Squares[j, i].PieceId;
    
                boardString.Append(verticalSeparator);
    
                if (pieceId != null)
                {
                    char symbol = ChessDictionary.PieceNames[pieceId.Value];
                    boardString.Append(symbol);
                    boardString.Append(' ');
                }
                else
                {
                    // Use different symbols for light and dark squares
                    string emptySquareSymbol = (i + j) % 2 == 0 ? emptyDarkSquareSymbol : emptyLightSquareSymbol;
    
                    boardString.Append(emptySquareSymbol);
                }
            }
    
            boardString.Append(verticalSeparator);
            boardString.Append('\n');
        }
    
        boardString.AppendLine();
    
        return boardString.ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square GetSquare(Location location)
    {
        byte x = (byte)(location.X - 1);
        byte y = (byte)(location.Y - 1);

        return Squares[x, y];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Board DeepCopy()
    {
        Board newBoard = new Board(true);

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                if (Squares[i, j].PieceId != null)
                {
                    newBoard.Squares[i, j].SetPiece(Squares[i, j].PieceId!.Value);
                }
            }
        }

        newBoard.CurrentMoveColor = CurrentMoveColor;
        newBoard.WhiteKingSquare = WhiteKingSquare;
        newBoard.BlackKingSquare = BlackKingSquare;

        return newBoard;
    }
}