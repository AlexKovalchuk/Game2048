// See https://aka.ms/new-console-template for more information

using Game2048.Core.Models;
using Game2048.Core.Infrastructure;
using Game2048.Core.Dtos;

Console.WriteLine("Hello, World! This is 2048 playground");
var board = new Board(4, new SystemRandom());
board.Init();
BoardSnapshot snapshot = board.GetSnapshot();
Console.WriteLine($"Game State = {board.State}");
snapshot.Tiles.ForEach(tile => Console.WriteLine($"tile.Column = {tile.Column}, tile.Row = {tile.Row}, tile.Value = {tile.Value}"));
