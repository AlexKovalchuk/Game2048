using System;
using System.Linq;
using System.Reflection;
using Game2048.Core.Dtos;
using Game2048.Core.Enums;
using Game2048.Core.Models;
using Game2048.Core.Abstractions;
using FluentAssertions;

namespace Game2048.Tests.Helpers;

public static class BoardTestTools
{
    private static readonly PropertyInfo TilesProp =
        typeof(Board).GetProperty("Tiles", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Cannot access Board.Tiles via reflection.");

    public static Board FromGrid(int[,] grid, IRandom rng)
    {
        int size = grid.GetLength(0);
        if (size != grid.GetLength(1))
            throw new ArgumentException("Grid must be square.");

        var board = new Board(size, rng);
        board.Clear();

        var tiles = (System.Collections.Generic.List<Tile>)TilesProp.GetValue(board)!;
        tiles.Clear();

        for (int r = 0; r < size; r++)
        for (int c = 0; c < size; c++)
        {
            var v = grid[r, c];
            if (v > 0)
                tiles.Add(new Tile { Row = (byte)r, Column = (byte)c, Value = (ushort)v });
        }

        return board;
    }

    public static int[,] ToGrid(Board board)
    {
        var snap = board.GetSnapshot();
        var grid = new int[snap.Size, snap.Size];
        foreach (var t in snap.Tiles)
            grid[t.Row, t.Column] = t.Value;
        return grid;
    }

    public static void AssertGrid(Board board, int[,] expected)
    {
        var actual = ToGrid(board);
        int n = actual.GetLength(0);
        n.Should().Be(expected.GetLength(0));
        for (int r = 0; r < n; r++)
        for (int c = 0; c < n; c++)
            actual[r, c].Should().Be(expected[r, c], $"cell [{r},{c}] should match");
    }
}