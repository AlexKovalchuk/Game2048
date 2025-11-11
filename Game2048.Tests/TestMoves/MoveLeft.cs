using Game2048.Core.Enums;
using Game2048.Tests.Helpers;
using Game2048.Tests.TestRandom;
using Xunit;

namespace Game2048.Tests.TestMoves;

public class MoveLeft
{
    [Fact]
    public void Left_Merges_First_Pair_And_Spawns_Deterministically()
    {
        // Initial board 4x4:
        // [2,2,0,0]
        // [0,0,0,0]
        // [0,0,0,0]
        // [0,0,0,0]
        var start = new int[4,4];
        start[0,0] = 2;
        start[0,1] = 2;

        // Скриптуємо RNG так, щоб Spawn після ходу з'явився у ПЕРШІЙ порожній клітинці,
        // а значення було 2 (NextDouble=0.95 -> 2).
        // Після ходу left рядок стане [4,0,0,0], тож порожні йдуть у порядку (0,1),(0,2),(0,3),...
        var rng = new ScriptedRandom(
            ints: new[] { 0 },      // індекс 0 -> (0,1)
            doubles: new[] { 0.1 } // 0.9..1 => 2
        );

        var board = BoardTestTools.FromGrid(start, rng);
        var scoreBefore = board.Score;

        // ДІЯ
        board.Move(MoveDirection.Left);

        // ОЧІКУВАНО:
        // [4,2,0,0]
        // решта нулі
        var expected = new int[4,4];
        expected[0,0] = 4;
        expected[0,1] = 2;

        BoardTestTools.AssertGrid(board, expected);
        Assert.Equal(scoreBefore + 4, board.Score);
    }
}