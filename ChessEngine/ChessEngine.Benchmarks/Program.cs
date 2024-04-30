using BenchmarkDotNet.Running;

namespace ChessEngine.Benchmarks;

public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmarks>();
    }
}