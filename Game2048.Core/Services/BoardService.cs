using Game2048.Core.Enums;

namespace Game2048.Core.Services;

public class BoardService
{
    public void NewGame()
    {
        Console.WriteLine("New Game Started"); 
    }
    
    public void Move(MoveDirection direction)
    {
        Console.WriteLine($"Move: {direction}");
    }
    
    public bool CanMove(MoveDirection direction)
    {
        Console.WriteLine($"Check if can move: {direction}");
        return true;
    }
    
    public void Undo()
    {
        Console.WriteLine("Undo last move");
    }

    public void GetSnapshot()
    {
        Console.WriteLine("Get current board snapshot");
    }
}