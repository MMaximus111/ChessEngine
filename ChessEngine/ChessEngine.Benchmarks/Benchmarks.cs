using BenchmarkDotNet.Attributes;
using ChessEngine.Entities;

namespace ChessEngine.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark]
    public void FirstMove()
    {
        Board board = new Board();
        Engine engine = new Engine();
        
        Move bestMove = engine.MakeTheBestMove(board, 8);
    }
}