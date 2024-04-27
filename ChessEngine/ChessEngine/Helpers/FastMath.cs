using System.Runtime.CompilerServices;

namespace ChessEngine.Helpers;

public class FastMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int value)
    {
        // Ensure value is positive using technique faster than System.Math.Abs().
        // See http://graphics.stanford.edu/~seander/bithacks.html#IntegerAbs.
        int mask = value >> 31;
        return (value ^ mask) - mask;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal Max(decimal value1, decimal value2) => value1 > value2 ? value1 : value2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal Min(decimal value1, decimal value2) => value1 < value2 ? value1 : value2;
}