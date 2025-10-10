namespace Game2048.Core.Models;

public class Tile
{
    public ushort Value { get; set; } // should be power of 2
    public byte Row { get; set; }
    public byte Column { get; set; }
    // public Guid Id { get; set; }
    // public bool JustSpawned { get; set; }
    // public bool JustMerged { get; set; }
}