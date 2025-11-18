using Game2048.Core.Enums;
using Game2048.Tests.Helpers;
using Game2048.Tests.TestRandom;
using Xunit;

namespace Game2048.Tests.TestMoves;

public class MoveUp
{
    [Fact]
    public void Up_Merges_First_Pair_And_Spawns_Deterministically()
    {
        // Колонка 0: [2,2,0,0]^T
        var start = new int[4,4];
        start[0,0] = 2;
        start[1,0] = 2;

        // Після руху вгору маємо (0,0)=4, решта порожні.
        // GetEmptyTiles() перебирає по рядках зліва-направо,
        // тож перша порожня клітинка — (0,1) => індекс 0.
        var rng = new ScriptedRandom(
            ints:    new[] { 0 },   // spawn у (0,1)
            doubles: new[] { 0.10 } // <0.9 ⇒ значення 2
        );

        var board = BoardTestTools.FromGrid(start, rng);
        var scoreBefore = board.Score;

        board.Move(MoveDirection.Up);

        var expected = new int[4,4];
        expected[0,0] = 4; // merge
        expected[0,1] = 2; // spawn

        BoardTestTools.AssertGrid(board, expected);
        Assert.Equal(scoreBefore + 4, board.Score);
    }
}