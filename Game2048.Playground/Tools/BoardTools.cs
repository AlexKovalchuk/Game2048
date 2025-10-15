using Game2048.Core.Dtos;

namespace Game2048.Playground.Tools;

public class BoardTools
{
    public void DrawTheBoard(int size, List<TileDto> tiles)
    {
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                var tile = tiles.FirstOrDefault(t => t.Row == r && t.Column == c);
                if (tile != null)
                {
                    if (c == size - 1) Console.Write($" | {tile.Value} |");
                    else Console.Write($" | {tile.Value}");
                }
                else
                {
                    if (c == size - 1) Console.Write($" | - |");
                    else Console.Write($" | -");
                }
            }
            Console.WriteLine();
        }
    }
}