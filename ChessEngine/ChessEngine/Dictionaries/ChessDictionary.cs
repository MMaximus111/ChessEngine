using System.Collections.Frozen;
using ChessEngine.Entities.Pieces;

namespace ChessEngine.Dictionaries;

public static class ChessDictionary
{
    public const int WhitePawnId = 1;
    public const int BlackPawnId = 2;
    public const int WhiteKnightId = 3;
    public const int BlackKnightId = 4;
    public const int WhiteBishopId = 5;
    public const int BlackBishopId = 6;
    public const int WhiteRookId = 7;
    public const int BlackRookId = 8;
    public const int WhiteQueenId = 9;
    public const int BlackQueenId = 10;
    public const int WhiteKingId = 11;
    public const int BlackKingId = 12;

    public static readonly Dictionary<byte, char> PieceNames = new Dictionary<byte, char>
    {
        { BlackKingId, '♔' },
        { BlackQueenId, '♕' },
        { BlackRookId, '♖' },
        { BlackBishopId, '♗' },
        { BlackKnightId, '♘' },
        { BlackPawnId, '♙' },
        { WhiteKingId, '♚' },
        { WhiteQueenId, '♛' },
        { WhiteRookId, '♜' },
        { WhiteBishopId, '♝' },
        { WhiteKnightId, '♞' },
        { WhitePawnId, '♟' }
    };

    public static readonly FrozenDictionary<byte, byte> PiecePrices = new Dictionary<byte, byte>
    {
        { BlackKingId, 0 },
        { BlackQueenId, 9 },
        { BlackRookId, 5 },
        { BlackBishopId, 3 },
        { BlackKnightId, 3 },
        { BlackPawnId, 1 },
        { WhiteKingId, 0 },
        { WhiteQueenId, 9 },
        { WhiteRookId, 5 },
        { WhiteBishopId, 3 },
        { WhiteKnightId, 3 },
        { WhitePawnId, 1 }
    }.ToFrozenDictionary();

    public static PieceColor GetColorByPieceId(byte pieceId)
    {
        return pieceId % 2 == 0 ? PieceColor.Black : PieceColor.White;
    }
}