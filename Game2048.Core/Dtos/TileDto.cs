namespace Game2048.Core.Dtos;

public class TileDto
{
    public ushort Value { get; set; } // should be power of 2
    public byte Row { get; set; }
    public byte Column { get; set; }
}