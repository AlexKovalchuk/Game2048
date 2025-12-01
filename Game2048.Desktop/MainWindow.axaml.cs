using Avalonia.Controls;
using Game2048.Desktop.ViewModels;
using KeyEventArgs = Avalonia.Input.KeyEventArgs;
using Avalonia.Input;

namespace Game2048.Desktop;

public partial class MainWindow : Window
{
    private GameViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();

        _vm = new GameViewModel();
        DataContext = _vm;

        this.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
            case Key.W:
                _vm.MoveUp();
                break;

            case Key.Down:
            case Key.S:
                _vm.MoveDown();
                break;

            case Key.Left:
            case Key.A:
                _vm.MoveLeft();
                break;

            case Key.Right:
            case Key.D:
                _vm.MoveRight();
                break;

            case Key.R:
                _vm.NewGame();
                break;
        }
    }
}