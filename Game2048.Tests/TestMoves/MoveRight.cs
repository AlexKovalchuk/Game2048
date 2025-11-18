using Game2048.Core.Enums;
using Game2048.Tests.Helpers;
using Game2048.Tests.TestRandom;

namespace Game2048.Tests.TestMoves;

public class MoveRight
{
    [Fact]
    public void Right_Merges_Last_Pair_And_Spawns_Deterministically()
    {
        // Старт:
        // [0,0,2,2]
        // [0,0,0,0]
        // [0,0,0,0]
        // [0,0,0,0]
        var start = new int[4,4];
        start[0,2] = 2;
        start[0,3] = 2;

        // Після Move(Right): рядок стане [0,0,0,4]
        // Порожні у першому рядку: (0,0),(0,1),(0,2)
        // Вибираємо першу порожню (index 0) і спавнимо 2 (double < 0.9)
        var rng = new ScriptedRandom(
            ints:    new[] { 0 },   // -> (0,0)
            doubles: new[] { 0.1 }  // -> 2
        );

        var board = BoardTestTools.FromGrid(start, rng);
        var scoreBefore = board.Score;

        board.Move(MoveDirection.Right);

        var expected = new int[4,4];
        expected[0,0] = 2; // spawn
        expected[0,3] = 4; // merge

        BoardTestTools.AssertGrid(board, expected);
        Assert.Equal(scoreBefore + 4, board.Score);
    }
}