using System.Runtime.CompilerServices;
using System.Text;
using ChessEngine.Entities.Pieces;
using ChessEngine.Entities.Pieces.Black;
using ChessEngine.Entities.Pieces.White;

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

               Piece? piece = null;

               if (!empty)
               {
                   piece = location switch
                   {
                       { X: 1, Y: 1 } => new WhiteRook(),
                       { X: 2, Y: 1 } => new WhiteKnight(),
                       { X: 3, Y: 1 } => new WhiteBishop(),
                       { X: 4, Y: 1 } => new WhiteQueen(),
                       { X: 5, Y: 1 } => new WhiteKing(),
                       { X: 6, Y: 1 } => new WhiteBishop(),
                       { X: 7, Y: 1 } => new WhiteKnight(),
                       { X: 8, Y: 1 } => new WhiteRook(),
                       { Y: 2 } => new WhitePawn(),
                       { Y: 7 } => new BlackPawn(),
                       { X: 1, Y: 8 } => new BlackRook(),
                       { X: 2, Y: 8 } => new BlackKnight(),
                       { X: 3, Y: 8 } => new BlackBishop(),
                       { X: 4, Y: 8 } => new BlackQueen(),
                       { X: 5, Y: 8 } => new BlackKing(),
                       { X: 6, Y: 8 } => new BlackBishop(),
                       { X: 7, Y: 8 } => new BlackKnight(),
                       { X: 8, Y: 8 } => new BlackRook(),
                       _ => null
                   };
                   
                   WhiteKingSquare = GetSquare(new Location(5, 1));
                   BlackKingSquare = GetSquare(new Location(5, 8));
               }

               Squares[i, j] = new Square(location, piece);
           }
       }
   }
   
   public Square? WhiteKingSquare { get; private set; }
   
   public Square? BlackKingSquare { get; private set; }
    
    public PieceColor CurrentMoveColor { get; private set; } = PieceColor.White;

    public Square[,] Squares { get; }
    
    // public void Move(string notation)
    // {
    //     if (notation.Length != 3)
    //     {
    //         throw new ArgumentException("Invalid notation.");
    //     }
    //
    //     char pieceName = notation[0];
    //     int x = GetXBasedOnLeter(notation[1]);
    //     int y = notation[2] - '0';
    //
    //     Location to = new Location(x, y);
    //
    //     for (int i = 0; i < 8; i++)
    //     {
    //         for (int j = 0; j < 8; j++)
    //         {
    //             Piece? piece = Squares[j, i].Piece;
    //
    //             if (piece != null && string.Equals(piece.Name[0].ToString(), pieceName.ToString(), StringComparison.OrdinalIgnoreCase) && piece.GetValidLocationsToMove(Squares[j, i].Location, this, true).Contains(to))
    //             {
    //                 Move(new Move(Squares[j, i].Location, to, piece));
    //                 return;
    //             }
    //         }
    //     }
    //
    //     throw new InvalidOperationException("No valid move found for the given notation.");
    // }

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
                Piece? piece = square.Piece;
        
                if (piece != null && piece.Color != kingColor)
                {
                    IEnumerable<Location> validLocationsToMove = piece.GetValidLocationsToMove(square.Location, this, false);
        
                    foreach (Location location in validLocationsToMove)
                    {
                        if (location.Equals(kingSquare.Location))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    
    public static bool IsLocationOnBoard(Location location)
    {
        return location.X >= 1 && location.X <= 8 && location.Y >= 1 && location.Y <= 8;
    }
    
    public IEnumerable<Move> GetAllPossibleMoves()
    {
        foreach (Square square in Squares)
        {
            Piece? piece = square.Piece;

            if (piece != null && piece.Color == CurrentMoveColor)
            {
                foreach (Location location in piece.GetValidLocationsToMove(square.Location, this, true))
                {
                    yield return new Move(square.Location, location, piece.Icon);
                }
            }
        }
    }
    
    public bool IsGameOver()
    {
        return !GetAllPossibleMoves().Any();
    }
    
    public bool IsKingInCheckmate(PieceColor kingColor)
    {
        return IsKingInCheck(kingColor) && IsGameOver();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Move(Move move)
    {
        Square fromSquare = GetSquare(move.From);
        Square toSquare = GetSquare(move.To);

        toSquare.SetPiece(fromSquare.Piece!);
        fromSquare.Clear();
        
        CurrentMoveColor = CurrentMoveColor == PieceColor.White ? PieceColor.Black : PieceColor.White;

        if (toSquare.Piece is King king)
        {
            if (king.Color == PieceColor.White)
            {
                WhiteKingSquare = toSquare;
            }
            else
            {
                BlackKingSquare = toSquare;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal GetPiecesPrice(PieceColor color)
    {
        decimal price = 0;
        
        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                Square square = Squares[i, j];

                if (square.Piece != null && square.Piece.Color == color)
                {
                    price += square.Piece.Price;
                    //price += (square.Piece.Price + (square.Piece.GetValidLocationsToMove(square.Location, this, true).Count() / (decimal)100));
                }
            }
        }

        return price;
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
                Piece? piece = Squares[j, i].Piece;
    
                boardString.Append(verticalSeparator);
    
                if (piece != null)
                {
                    char symbol = piece.Icon;
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
    
    public Square GetSquare(Location location)
    {
        int x = location.X - 1;
        int y = location.Y - 1;

        return Squares[x, y];
    }
    
    public Board DeepCopy()
    {
        Board newBoard = new Board(true);

        for (byte i = 0; i < 8; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                if (Squares[i, j].Piece != null)
                {
                    newBoard.Squares[i, j].SetPiece(Squares[i, j].Piece!.DeepCopy());
                }
            }
        }

        newBoard.CurrentMoveColor = CurrentMoveColor;
        newBoard.WhiteKingSquare = WhiteKingSquare;
        newBoard.BlackKingSquare = BlackKingSquare;

        return newBoard;
    }
    
    // private static int GetXBasedOnLeter(char letter)
    // {
    //     return char.ToLower(letter) switch
    //     {
    //         'a' => 1,
    //         'b' => 2,
    //         'c' => 3,
    //         'd' => 4,
    //         'e' => 5,
    //         'f' => 6,
    //         'g' => 7,
    //         'h' => 8,
    //         _ => throw new ArgumentOutOfRangeException("Invalid letter.")
    //     };
    // }
}