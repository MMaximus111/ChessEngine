using ChessEngine.Entities;

namespace ChessEngine.Helpers;

public static class MoveHelper
{
    public static IEnumerable<Move> OrderByPriorityDesc(this IEnumerable<Move> moves)
    {
        return moves.OrderByDescending(x => x.Priority);
    }
}