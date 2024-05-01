using System.Collections.Frozen;
using ChessEngine.Entities.Pieces;

namespace ChessEngine.Dictionaries;

public static class ChessDictionary
{
    public const byte NoneId = 0;
    public const byte WhitePawnId = 1;
    public const byte BlackPawnId = 2;
    public const byte WhiteKnightId = 3;
    public const byte BlackKnightId = 4;
    public const byte WhiteBishopId = 5;
    public const byte BlackBishopId = 6;
    public const byte WhiteRookId = 7;
    public const byte BlackRookId = 8;
    public const byte WhiteQueenId = 9;
    public const byte BlackQueenId = 10;
    public const byte WhiteKingId = 11;
    public const byte BlackKingId = 12;

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