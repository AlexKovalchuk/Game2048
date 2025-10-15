using Game2048.Core.Enums;

namespace Game2048.Core.Dtos;

public class BoardSnapshot
{
    public int Size { get; set; }
    public int Score { get; set; }
    public GameState State { get; set; }
    public List<TileDto> Tiles { get; set; } = new List<TileDto>();
}