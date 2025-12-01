using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Game2048.Core.Enums;
using Game2048.Core.Infrastructure;
using Game2048.Core.Models;
using System.Windows.Input;
using Game2048.Desktop.Commands;

namespace Game2048.Desktop.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
    private readonly Board _board;
    public ObservableCollection<TileViewModel> Tiles { get; } = new();
    public ICommand NewGameCommand { get; }
    public ICommand MoveLeftCommand { get; }
    public ICommand MoveRightCommand { get; }
    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }
    public ICommand UndoCommand { get; } // some day
    public bool CanUndo
    {
        get => _canUndo;
        private set => SetField(ref _canUndo, value);
    }
    private bool _canUndo;

    private int _score;

    public int Score
    {
        get => _score;
        private set
        {
            if (_score == value) return;
            _score = value;
            OnPropertyChanged();
        }
    }

    public int BoardSize => _board.Size;

    public event PropertyChangedEventHandler? PropertyChanged;

    public GameViewModel()
    {
        // for now, hardcoded size and random generator
        _board = new Board(4, new SystemRandom());
        NewGameCommand  = new RelayCommand(NewGame);
        MoveLeftCommand  = new RelayCommand(MoveLeft);
        MoveRightCommand = new RelayCommand(MoveRight);
        MoveUpCommand    = new RelayCommand(MoveUp);
        MoveDownCommand  = new RelayCommand(MoveDown);
        UndoCommand = new RelayCommand(
            execute: () => { /* TODO: implement Undo, some day */ },
            canExecute: () => CanUndo
        );
        NewGame();
    }

    public void NewGame()
    {
        _board.Init();
        RefreshFromBoard();
    }

    private void RefreshFromBoard()
    {
        Tiles.Clear();

        var snapshot = _board.GetSnapshot();

        // Створюємо словник існуючих тайлів
        var dict = snapshot.Tiles.ToDictionary(t => (t.Row, t.Column));

        for (int r = 0; r < BoardSize; r++)
        {
            for (int c = 0; c < BoardSize; c++)
            {
                if (dict.TryGetValue(((byte)r, (byte)c), out var tile))
                {
                    Tiles.Add(new TileViewModel
                    {
                        Row = (byte)r,
                        Column = (byte)c,
                        Value = tile.Value
                    });
                }
                else
                {
                    Tiles.Add(new TileViewModel
                    {
                        Row = (byte)r,
                        Column = (byte)c,
                        Value = 0
                    });
                }
            }
        }

        Score = _board.Score;
    }


    public string StateText => 
        _board.State switch
        {
            GameState.InProgress => "Keep playing...",
            GameState.Won => "You win!",
            GameState.Lost => "Game over? Try again.",
            _ => ""
        };

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    public void MoveLeft()
    {
        if (_board.State != GameState.InProgress) return; 
        
        _board.Move(MoveDirection.Left);
        RefreshFromBoard();
        
        if (_board.State != GameState.InProgress)
            OnPropertyChanged(nameof(StateText));
    }

    public void MoveRight()
    {
        if (_board.State != GameState.InProgress) return; 
        
        _board.Move(MoveDirection.Right);
        RefreshFromBoard();
        
        if (_board.State != GameState.InProgress)
            OnPropertyChanged(nameof(StateText));
    }

    public void MoveUp()
    {
        if (_board.State != GameState.InProgress) return; 
        
        _board.Move(MoveDirection.Up);
        RefreshFromBoard();
        
        if (_board.State != GameState.InProgress)
            OnPropertyChanged(nameof(StateText));
    }

    public void MoveDown()
    {
        if (_board.State != GameState.InProgress) return; 
        
        _board.Move(MoveDirection.Down);
        RefreshFromBoard();
        
        if (_board.State != GameState.InProgress)
            OnPropertyChanged(nameof(StateText));
    }


}