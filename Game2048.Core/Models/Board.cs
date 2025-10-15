using Game2048.Core.Abstractions;
using Game2048.Core.Dtos;
using Game2048.Core.Enums;

namespace Game2048.Core.Models;

public class Board
{
    List<Tile> Tiles { get; }
    public int Size { get; private set; }
    public int Score { get; private set; }
    public GameState State { get; private set; }
    private IRandom Random { get; set; }

    public Board(int size, IRandom random)
    {
        Size = size >=2 ? size : 4;
        Score = 0;
        State = GameState.InProgress;
        Tiles = new List<Tile>();
        Random = random;
    }

    public void Clear()
    {
        Score = 0;
        State = GameState.InProgress;
        Tiles.Clear();
    }

    private List<Tile> GetEmptyTiles()
    {
        List<Tile> emptyTiles = new List<Tile>();
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                if (!Tiles.Any(x => x.Row == i && x.Column == j))
                {
                    emptyTiles.Add(new Tile { Row = (byte)i, Column = (byte)j, Value = 0 });
                }
            }
        }
        return emptyTiles;
    }
    
    private void Spawn()
    {
        List <Tile> emptyTiles = GetEmptyTiles();
        if (emptyTiles.Count == 0)
        {
            Console.WriteLine($"Cannot spawn a tile, no empty space");
            return;
        }
        int randomValue = Random.NextDouble() < 0.9 ? 2 : 4;
        int randomEmptyTileIndex = Random.Next(0, emptyTiles.Count);
        emptyTiles[randomEmptyTileIndex].Value = (ushort)randomValue;
        Tiles.Add(emptyTiles[randomEmptyTileIndex]);
    }
    
    public void Init()
    {
        Clear();
        Spawn();
        Spawn();
    }
    
    public BoardSnapshot GetSnapshot()
    {
        BoardSnapshot snapshot = new BoardSnapshot
        {
            Size = Size,
            Score = Score,
            State = State,
            Tiles = Tiles.Select(t => new TileDto { Row = t.Row, Column = t.Column, Value = t.Value }).ToList()
        };
        return snapshot;
    }
}
