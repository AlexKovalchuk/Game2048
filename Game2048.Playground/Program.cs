// See https://aka.ms/new-console-template for more information

using Game2048.Core.Models;
using Game2048.Core.Infrastructure;

Console.WriteLine("Hello, World! This is 2048 playground");
var board = new Board(4, new SystemRandom());
board.Clear();