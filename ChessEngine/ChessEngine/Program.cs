using System.Diagnostics;
using System.Text;
using ChessEngine;
using ChessEngine.Entities;

Console.OutputEncoding = Encoding.UTF8;

Board board = new Board(false);

Console.WriteLine("Welcome to the Chess Engine!");

while (true)
{
    Console.WriteLine($"Current move: {board.CurrentMoveColor}");
    Console.WriteLine("Engine is thinking...");
        
    Engine engine = new Engine();
        
    Stopwatch sw = Stopwatch.StartNew();
    
    Move bestMove = engine.MakeTheBestMove(board, 8);

    sw.Stop();
    
    Console.WriteLine($"Engine move: {bestMove}");
    Console.WriteLine($"Score: {engine.Evaluate(board)}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    Console.WriteLine($"Evaluations: {engine.GetEvaluationsCount()}");
    
    Console.WriteLine(board);
}