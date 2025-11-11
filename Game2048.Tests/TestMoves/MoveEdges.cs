using Game2048.Core.Enums;
using Game2048.Tests.Helpers;
using Game2048.Tests.TestRandom;
using Xunit;

namespace Game2048.Tests.TestMoves;

public class MoveEdges
{
    // ---------- LEFT ----------
    [Fact]
    public void Left_Edge_Chain_2222_To_44_And_Spawn()
    {
        // [2,2,2,2] -> [4,4,0,0] -> spawn at first empty (0,2) -> [4,4,2,0]
        var g = new int[4,4];
        g[0,0]=2; g[0,1]=2; g[0,2]=2; g[0,3]=2;

        var rng = new ScriptedRandom(
            ints:    new[] { 0 },  // first empty after move is (0,2)
            doubles: new[] { 0.10 } // => 2
        );
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Left);

        var exp = new int[4,4];
        exp[0,0]=4; exp[0,1]=4; exp[0,2]=2;
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 8, board.Score); // 2 merges: 2+2=4, 2+2=4
    }

    [Fact]
    public void Left_Edge_Three_2220_To_42_And_Spawn()
    {
        // [2,2,2,0] -> [4,2,0,0] -> spawn at (0,2) -> [4,2,2,0]
        var g = new int[4,4];
        g[0,0]=2; g[0,1]=2; g[0,2]=2;

        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 }); // (0,2), value 2
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Left);

        var exp = new int[4,4];
        exp[0,0]=4; exp[0,1]=2; exp[0,2]=2;
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 4, board.Score);
    }

    [Fact]
    public void Left_Edge_Hole_2022_To_42_And_Spawn()
    {
        // [2,0,2,2] -> compress -> [2,2,2,0] -> merge -> [4,2,0,0] -> spawn at (0,2)
        var g = new int[4,4];
        g[0,0]=2; g[0,2]=2; g[0,3]=2;

        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 }); // (0,2), value 2
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Left);

        var exp = new int[4,4];
        exp[0,0]=4; exp[0,1]=2; exp[0,2]=2;
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 4, board.Score);
    }

    [Fact]
    public void Left_NoMove_NoSpawn()
    {
        // Already packed, no merges: [2,4,8,16] -> no change and NO spawn
        var g = new int[4,4];
        g[0,0]=2; g[0,1]=4; g[0,2]=8; g[0,3]=16;

        // Even if RNG is ready, it must NOT be used because board doesn't change
        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 });
        var board = BoardTestTools.FromGrid(g, rng);

        var before = BoardTestTools.ToGrid(board);
        board.Move(MoveDirection.Left);
        BoardTestTools.AssertGrid(board, before);
    }

    // ---------- RIGHT (mirrors) ----------
    [Fact]
    public void Right_Edge_Chain_2222_To_44_And_Spawn()
    {
        // [2,2,2,2] -> Right => [0,0,4,4] -> spawn at first empty (0,0) => [2,0,4,4]
        var g = new int[4,4];
        g[0,0]=2; g[0,1]=2; g[0,2]=2; g[0,3]=2;

        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 }); // (0,0), value 2
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Right);

        var exp = new int[4,4];
        exp[0,0]=2; exp[0,2]=4; exp[0,3]=4;
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 8, board.Score);
    }

    // ---------- UP / DOWN (one focused case each) ----------
    [Fact]
    public void Up_Edge_Chain_Column_2222_To_44_And_Spawn()
    {
        // Column 0: [2,2,2,2]^T -> Up => [4,4,0,0]^T in col0 -> spawn at (0,1)
        var g = new int[4,4];
        g[0,0]=2; g[1,0]=2; g[2,0]=2; g[3,0]=2;

        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 }); // (0,1), value 2
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Up);

        var exp = new int[4,4];
        exp[0,0]=4; exp[1,0]=4; // merges in column
        exp[0,1]=2;             // spawn by row-major empty order
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 8, board.Score);
    }

    [Fact]
    public void Down_Edge_Chain_Column_2222_To_44_And_Spawn()
    {
        // Column 0: [2,2,2,2]^T -> Down => [0,0,4,4]^T in col0 -> spawn at (0,1) first empty? No, first empty is (0,0)!
        var g = new int[4,4];
        g[0,0]=2; g[1,0]=2; g[2,0]=2; g[3,0]=2;

        var rng = new ScriptedRandom(ints: new[] { 0 }, doubles: new[] { 0.10 }); // (0,0), value 2
        var board = BoardTestTools.FromGrid(g, rng);
        var score0 = board.Score;

        board.Move(MoveDirection.Down);

        var exp = new int[4,4];
        exp[2,0]=4; exp[3,0]=4; // merges at bottom
        exp[0,0]=2;             // spawn (first empty by row-major)
        BoardTestTools.AssertGrid(board, exp);
        Assert.Equal(score0 + 8, board.Score);
    }
}
