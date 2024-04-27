using System.Diagnostics;
using System.Text;
using ChessEngine;
using ChessEngine.Entities;

Console.OutputEncoding = Encoding.UTF8;

Board board = new Board();

Console.WriteLine("Welcome to the Chess Engine!");

while (true)
{
    Console.WriteLine($"Current move: {board.CurrentMoveColor}");
    Console.WriteLine("Engine is thinking...");
        
    Engine engine = new Engine();
        
    Stopwatch sw = Stopwatch.StartNew();
    
    Move bestMove = engine.MakeTheBestMove(board, 6);

    sw.Stop();
    
    Console.WriteLine($"Engine move: {bestMove}");
    Console.WriteLine($"Score: {engine.Evaluate(board)}");
    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    
    Console.WriteLine(board);
    
    // Console.WriteLine();
    // Console.WriteLine(board);
    // Console.WriteLine();
    //
    // Console.Write("Enter move: ");
    // string move = Console.ReadLine()!;
    //
    // if (move == "ENGINE" || move == "engine" || move == "e" || move == "E" || move == "")
    // {
    //     Console.WriteLine("Engine is thinking...");
    //     
    //     Engine engine = new Engine();
    //     
    //     Move bestMove = engine.MakeTheBestMove(board, 6);
    //
    //     Console.WriteLine($"Engine move: {bestMove}");
    //     Console.WriteLine($"Evaluation: {engine.Evaluate(board)}");
    // }

    // try
    // {
    //     board.Move(move);
    //     
    //     Console.WriteLine(board);
    // }
    // catch (Exception ex)
    // {
    //     Console.WriteLine(ex.Message);
    // }
}