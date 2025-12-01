// See https://aka.ms/new-console-template for more information

using Game2048.Core.Models;
using Game2048.Core.Infrastructure;
using Game2048.Core.Dtos;
using Game2048.Core.Enums;
using Game2048.Playground.Tools;

Console.WriteLine("Hello, World! This is 2048 playground");
var board = new Board(4, new SystemRandom());
// board.Init();
board.DebugSpawnMove();
BoardSnapshot snapshot = board.GetSnapshot();
Console.WriteLine($"Game State = {board.State}, Score = {snapshot.Score}");
snapshot.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
var tools = new BoardTools();
tools.DrawTheBoard(snapshot.Size, snapshot.Tiles);

board.Move(MoveDirection.Left);
BoardSnapshot snapshot2 = board.GetSnapshot();
snapshot2.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
tools.DrawTheBoard(snapshot2.Size, snapshot2.Tiles);
Console.WriteLine($"Game State = {board.State}, Score = {snapshot2.Score}");

board.Move(MoveDirection.Right);
BoardSnapshot snapshot3 = board.GetSnapshot();
snapshot3.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
tools.DrawTheBoard(snapshot3.Size, snapshot3.Tiles);
Console.WriteLine($"Game State = {board.State}, Score = {snapshot3.Score}");

board.Move(MoveDirection.Down);
BoardSnapshot snapshot4 = board.GetSnapshot();
snapshot4.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
tools.DrawTheBoard(snapshot4.Size, snapshot4.Tiles);
Console.WriteLine($"Game State = {board.State}, Score = {snapshot4.Score}");

board.Move(MoveDirection.Up);
BoardSnapshot snapshot5 = board.GetSnapshot();
snapshot5.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
tools.DrawTheBoard(snapshot5.Size, snapshot5.Tiles);
Console.WriteLine($"Game State = {board.State}, Score = {snapshot5.Score}");
