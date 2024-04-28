namespace ChessEngine.Entities;

public enum MovePriority : byte
{
    KingMove = 1,
    Default = 2,
    Check = 3,
    PieceCapture = 4
}