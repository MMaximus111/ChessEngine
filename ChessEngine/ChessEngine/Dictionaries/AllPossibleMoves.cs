using System.Collections.Frozen;
using ChessEngine.Entities;

namespace ChessEngine.Dictionaries;

public static class AllPossibleMoves
{
    static AllPossibleMoves()
    {
        Bishop = GenerateBishopMoves();
        Queen = GenerateQueenMoves();
        Knight = GenerateKnightMoves();
        Rook = GenerateRookMoves();
        King = GenerateKingMoves();
        AttackLocations = GenerateAllControlledSquares();
    }

    public static readonly FrozenDictionary<Location, Location[][]> Bishop;
    public static readonly FrozenDictionary<Location, Location[][]> Queen;
    public static readonly FrozenDictionary<Location, Location[][]> Knight;
    public static readonly FrozenDictionary<Location, Location[][]> Rook;
    public static readonly FrozenDictionary<Location, Location[][]> King;
    public static readonly FrozenDictionary<Location, Location[]> AttackLocations;

    private static readonly int[][] BishopDirections = new int[][]
    {
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };
    
    private static readonly int[][] QueenDirections = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 },
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };

    private static readonly int[][] KnightDirections = new int[][]
    {
        new int[] { -2, -1 }, new int[] { -2, 1 }, new int[] { -1, -2 }, new int[] { -1, 2 },
        new int[] { 1, -2 }, new int[] { 1, 2 }, new int[] { 2, -1 }, new int[] { 2, 1 }
    };
    
    private static readonly int[][] RookDirections = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 }
    };
    
    private static readonly int[][] KingDirections = new int[][]
    {
        new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 },
        new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 }
    };
    
    private static FrozenDictionary<Location, Location[]> GenerateAllControlledSquares()
    {
        Dictionary<Location, Location[]> allControlledSquares = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location> controlledSquares = new List<Location>();

                controlledSquares.AddRange(Bishop[currentLocation].SelectMany(moves => moves));
                controlledSquares.AddRange(Queen[currentLocation].SelectMany(moves => moves));
                controlledSquares.AddRange(Knight[currentLocation].SelectMany(moves => moves));
                controlledSquares.AddRange(Rook[currentLocation].SelectMany(moves => moves));
                controlledSquares.AddRange(King[currentLocation].SelectMany(moves => moves));

                allControlledSquares[currentLocation] = controlledSquares.Distinct().ToArray();
            }
        }

        return allControlledSquares.ToFrozenDictionary();
    }
    
    private static FrozenDictionary<Location, Location[][]> GenerateKingMoves()
    {
        Dictionary<Location, Location[][]> kingMoves = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location[]> validLocationsToMove = new List<Location[]>();

                foreach (int[] direction in KingDirections)
                {
                    Location newLocation = new Location((byte)(currentLocation.X + direction[0]), (byte)(currentLocation.Y + direction[1]));

                    if (Board.IsLocationOnBoard(newLocation))
                    {
                        validLocationsToMove.Add(new Location[] { newLocation });
                    }
                }

                kingMoves[currentLocation] = validLocationsToMove.ToArray();
            }
        }

        return kingMoves.ToFrozenDictionary();
    }
    
    private static FrozenDictionary<Location, Location[][]> GenerateRookMoves()
    {
        Dictionary<Location, Location[][]> rookMoves = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location[]> validLocationsToMove = new List<Location[]>();

                foreach (int[] direction in RookDirections)
                {
                    List<Location> directionMoves = new List<Location>();

                    for (byte i = 1; i < 8; i++)
                    {
                        Location newLocation = new Location((byte)(currentLocation.X + direction[0] * i), (byte)(currentLocation.Y + direction[1] * i));

                        if (Board.IsLocationOnBoard(newLocation))
                        {
                            directionMoves.Add(newLocation);
                        }
                        else
                        {
                            break;
                        }
                    }

                    validLocationsToMove.Add(directionMoves.ToArray());
                }

                rookMoves[currentLocation] = validLocationsToMove.ToArray();
            }
        }

        return rookMoves.ToFrozenDictionary();
    }
    
    private static FrozenDictionary<Location, Location[][]> GenerateBishopMoves()
    {
        Dictionary<Location, Location[][]> bishopMoves = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location[]> validLocationsToMove = new List<Location[]>();

                foreach (int[] direction in BishopDirections)
                {
                    List<Location> directionMoves = new List<Location>();

                    for (byte i = 1; i < 8; i++)
                    {
                        Location newLocation = new Location((byte)(currentLocation.X + direction[0] * i), (byte)(currentLocation.Y + direction[1] * i));

                        if (Board.IsLocationOnBoard(newLocation))
                        {
                            directionMoves.Add(newLocation);
                        }
                        else
                        {
                            break;
                        }
                    }

                    validLocationsToMove.Add(directionMoves.ToArray());
                }

                bishopMoves[currentLocation] = validLocationsToMove.ToArray();
            }
        }

        return bishopMoves.ToFrozenDictionary();
    }

    private static FrozenDictionary<Location, Location[][]> GenerateQueenMoves()
    {
        Dictionary<Location, Location[][]> queenMoves = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location[]> validLocationsToMove = new List<Location[]>();

                foreach (int[] direction in QueenDirections)
                {
                    List<Location> directionMoves = new List<Location>();

                    for (byte i = 1; i < 8; i++)
                    {
                        Location newLocation = new Location((byte)(currentLocation.X + direction[0] * i), (byte)(currentLocation.Y + direction[1] * i));

                        if (Board.IsLocationOnBoard(newLocation))
                        {
                            directionMoves.Add(newLocation);
                        }
                        else
                        {
                            break;
                        }
                    }

                    validLocationsToMove.Add(directionMoves.ToArray());
                }

                queenMoves[currentLocation] = validLocationsToMove.ToArray();
            }
        }
        
        return queenMoves.ToFrozenDictionary();
    }

    private static FrozenDictionary<Location, Location[][]> GenerateKnightMoves()
    {
        Dictionary<Location, Location[][]> knightMoves = new();

        for (byte x = 1; x <= 8; x++)
        {
            for (byte y = 1; y <= 8; y++)
            {
                Location currentLocation = new Location(x, y);

                List<Location[]> validLocationsToMove = new List<Location[]>();

                foreach (int[] direction in KnightDirections)
                {
                    Location newLocation = new Location((byte)(currentLocation.X + direction[0]), (byte)(currentLocation.Y + direction[1]));

                    if (Board.IsLocationOnBoard(newLocation))
                    {
                        validLocationsToMove.Add(new Location[] { newLocation });
                    }
                }

                knightMoves[currentLocation] = validLocationsToMove.ToArray();
            }
        }

        return knightMoves.ToFrozenDictionary();
    }
}
