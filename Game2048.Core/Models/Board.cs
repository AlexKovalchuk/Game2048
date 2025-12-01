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
    
    public void Spawn()
    {
        List <Tile> emptyTiles = GetEmptyTiles();
        if (emptyTiles.Count == 0)
        {
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

    public void DebugSpawnMove()
    {
        List <Tile> emptyTiles = GetEmptyTiles();
        emptyTiles[0].Value = 2;
        emptyTiles[1].Value = 2;
        emptyTiles[2].Value = 2;
        emptyTiles[3].Value = 2;
        Tiles.Add(emptyTiles[0]);
        Tiles.Add(emptyTiles[1]);
        Tiles.Add(emptyTiles[2]);
        Tiles.Add(emptyTiles[3]);
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

    private bool HasChangedTiles(List<Tile> prevList, List<Tile> nextList)
    {
        var prev = prevList.ToDictionary(t => (t.Row, t.Column), t => t.Value); 
        var next = nextList.ToDictionary(t => (t.Row, t.Column), t => t.Value); 
        if (prev.Count != next.Count) return true;
        foreach (var kvPrev in prev)
        {
            if (!next.TryGetValue(kvPrev.Key, out var valueNext) || valueNext != kvPrev.Value)
                return true;
        }
        return false;
    }

    private bool CanMove(MoveDirection direction)
    {
        List<Tile> tilesSorted = Tiles.OrderBy(t => t.Row).ThenBy(t => t.Column).ToList();
        
        if (direction == MoveDirection.Left)
        {
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                List<Tile> rowTiles = tilesSorted
                    .Where(t => t.Row == rowIndex)
                    .OrderBy(t => t.Column)
                    .ToList();
                if(rowTiles.Count == 0) continue;
                for (int colIndex = 0; colIndex < rowTiles.Count; colIndex++)
                {
                    if (colIndex + 1 < rowTiles.Count && rowTiles[colIndex].Value == rowTiles[colIndex+1].Value)
                    {
                        return true;
                    }
                    
                    if (rowTiles[colIndex].Column != colIndex)
                    {
                        return true;
                    }
                }
            }
        }
        
        else if (direction == MoveDirection.Right)
        {
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                List<Tile> rowTiles = tilesSorted
                    .Where(t => t.Row == rowIndex)
                    .OrderByDescending(t => t.Column)
                    .ToList();
                int rowTilesCount = rowTiles.Count;
                if(rowTilesCount == 0) continue;
                for (int colIndex = 0; colIndex < rowTilesCount; colIndex++)
                {
                    if (colIndex + 1 < rowTilesCount && rowTiles[colIndex].Value == rowTiles[colIndex+1].Value)
                    {
                        return true;
                    }
                    
                    if (rowTiles[colIndex].Column != Size - rowTilesCount + colIndex)
                    {
                        return true;
                    }
                }
            }
        }

        else if (direction == MoveDirection.Up)
        {
            for (int colIndex = 0; colIndex < Size; colIndex++)
            {
                List<Tile> colTiles = tilesSorted
                    .Where(t => t.Column == colIndex)
                    .OrderBy(t => t.Row)
                    .ToList();
                if(colTiles.Count == 0) continue;
                for (int rowIndex = 0; rowIndex < colTiles.Count; rowIndex++)
                {
                    if (rowIndex + 1 < colTiles.Count && colTiles[rowIndex].Value == colTiles[rowIndex+1].Value)
                    {
                        return true;
                    }
                    
                    if (colTiles[rowIndex].Row != rowIndex)
                    {
                        return true;
                    }
                }
            }
        }
        
        else if (direction == MoveDirection.Down)
        {
            for (int colIndex = 0; colIndex < Size; colIndex++)
            {
                List<Tile> colTiles = tilesSorted
                    .Where(t => t.Column == colIndex)
                    .OrderByDescending(t => t.Row)
                    .ToList();
                int colTilesCount = colTiles.Count;
                if(colTilesCount == 0) continue;
                for (int rowIndex = 0; rowIndex < colTilesCount; rowIndex++)
                {
                    if (rowIndex + 1 < colTilesCount && colTiles[rowIndex].Value == colTiles[rowIndex+1].Value)
                    {
                        return true;
                    }
                    
                    if (colTiles[rowIndex].Row != Size - colTilesCount + rowIndex)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    
    private bool CanMoveGeneral()
    {
        List<Tile> tilesSorted = Tiles.OrderBy(t => t.Row).ThenBy(t => t.Column).ToList();
        for (int rowIndex = 0; rowIndex < Size; rowIndex++)
        {
            List<Tile> rowTiles = tilesSorted.Where(t => t.Row == rowIndex).ToList();
            for (int colIndex = 0; colIndex < rowTiles.Count; colIndex++)
            {
                if (colIndex + 1 < rowTiles.Count &&
                    rowTiles[colIndex].Value == rowTiles[colIndex+1].Value && 
                    rowTiles[colIndex].Value != 0)
                {
                    return true;
                }
            }
        }
        for (int colIndex = 0; colIndex < Size; colIndex++)
        {
            List<Tile> colTiles = tilesSorted.Where(t => t.Column == colIndex).ToList();
            for (int rowIndex = 0; rowIndex < colTiles.Count; rowIndex++)
            {
                if (rowIndex + 1 < colTiles.Count &&
                    colTiles[rowIndex].Value == colTiles[rowIndex+1].Value && 
                    colTiles[rowIndex].Value != 0)
                {
                    return true;
                }
            }
        }

        if (Tiles.Count < Size * Size) return true;
        
        return false;
    }
    
    private void UpdateGameState()
    {
        if (!CanMoveGeneral())
        {
            State = GameState.Lost;
        }
        else if(Tiles.Any(t => t.Value == 8096))
        {
            State = GameState.Won;
        }
        else 
        {
            State = GameState.InProgress;
        }
    }
    
    public void Move(MoveDirection direction)
    {
        List<Tile> prevTiles = Tiles
            .Select(t => new Tile { Row = t.Row, Column = t.Column, Value = t.Value })
            .ToList();
        List<Tile> tilesSorted = Tiles.OrderBy(t => t.Row).ThenBy(t => t.Column).ToList();

        if (direction == MoveDirection.Left)
        {
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                List<Tile> rowTiles = tilesSorted.Where(t => t.Row == rowIndex)
                    .OrderBy(t => t.Column)
                    .ToList();
                if(rowTiles.Count == 0) continue;

                for (int colIndex = 0; colIndex < rowTiles.Count; colIndex++)
                {
                    if (colIndex + 1 < rowTiles.Count && rowTiles[colIndex].Value == rowTiles[colIndex+1].Value)
                    {
                        rowTiles[colIndex].Value *= 2;
                        Score += rowTiles[colIndex].Value;
                        rowTiles[colIndex + 1].Value = 0;
                        rowTiles.RemoveAt(colIndex+1);
                    }
                    rowTiles[colIndex].Column = (byte)colIndex;
                }
            }
        }
        
        else if (direction == MoveDirection.Right)
        {
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                List<Tile> rowTiles = tilesSorted
                    .Where(t => t.Row == rowIndex)
                    .OrderByDescending(t => t.Column)
                    .ToList();

                if(rowTiles.Count == 0) continue;
                
                for (int colIndex = 0; colIndex < rowTiles.Count; colIndex++)
                {
                    if (colIndex + 1 < rowTiles.Count && rowTiles[colIndex].Value == rowTiles[colIndex+1].Value)
                    {
                        rowTiles[colIndex].Value *= 2;
                        Score += rowTiles[colIndex].Value;
                        rowTiles[colIndex + 1].Value = 0;
                        rowTiles.RemoveAt(colIndex+1);
                    }
                    rowTiles[colIndex].Column = (byte)(Size - (colIndex+1));
                }
            }
        }
        
        else if (direction == MoveDirection.Up)
        {
            for (int colIndex = 0; colIndex < Size; colIndex++)
            {
                List<Tile> colTiles = tilesSorted
                    .Where(t => t.Column == colIndex)
                    .OrderBy(t => t.Row)
                    .ToList();
                if(colTiles.Count == 0) continue;

                for (int rowIndex = 0; rowIndex < colTiles.Count; rowIndex++)
                {
                    if (rowIndex + 1 < colTiles.Count && colTiles[rowIndex].Value == colTiles[rowIndex+1].Value)
                    {
                        colTiles[rowIndex].Value *= 2;
                        Score += colTiles[rowIndex].Value;
                        colTiles[rowIndex + 1].Value = 0;
                        colTiles.RemoveAt(rowIndex+1);
                    }
                    colTiles[rowIndex].Row = (byte)rowIndex;
                }
            }
        }
        
        else if (direction == MoveDirection.Down)
        {
            for (int colIndex = 0; colIndex < Size; colIndex++)
            {
                List<Tile> colTiles = tilesSorted
                    .Where(t => t.Column == colIndex)
                    .OrderByDescending(t => t.Row)
                    .ToList();

                if(colTiles.Count == 0) continue;
                
                for (int rowIndex = 0; rowIndex < colTiles.Count; rowIndex++)
                {
                    if (rowIndex + 1 < colTiles.Count && colTiles[rowIndex].Value == colTiles[rowIndex+1].Value)
                    {
                        colTiles[rowIndex].Value *= 2;
                        Score += colTiles[rowIndex].Value;
                        colTiles[rowIndex + 1].Value = 0;
                        colTiles.RemoveAt(rowIndex+1);
                    }
                    colTiles[rowIndex].Row = (byte)(Size - (rowIndex+1));
                }
            }
        }
        

        Tiles.RemoveAll(t => t.Value == 0);
        UpdateGameState();
        if (HasChangedTiles(prevTiles, Tiles) && State == GameState.InProgress) Spawn();
    }
}
