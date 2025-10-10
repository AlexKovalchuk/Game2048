using Game2048.Core.Abstractions;
using Game2048.Core.Enums;

namespace Game2048.Core.Models;

public class Board
{
    List<Tile> Tiles { get; }
    public int Size { get; private set; }
    public int Score { get; private set; }
    public GameState State { get; private set; }
    IRandom Random { get; set; }

    public Board(int size, IRandom random)
    {
        Size = size >=2 ? size : 4;
        Score = 0;
        State = GameState.InProgress;
        Tiles = new List<Tile>();
        Random = random;
        
        if (size < 2) throw new ArgumentException("Board size must be >= 2");
    }

    public void Clear()
    {
        Score = 0;
        State = GameState.InProgress;
        Tiles?.Clear();
    }
}
