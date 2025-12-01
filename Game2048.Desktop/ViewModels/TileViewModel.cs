using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;

namespace Game2048.Desktop.ViewModels;

public class TileViewModel : INotifyPropertyChanged
{
    private ushort _value;

    public byte Row { get; set; }
    public byte Column { get; set; }

    public ushort Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(Foreground));
        }
    }
    
    public IBrush Background => Value switch
    {
        0    => Brushes.Transparent,
        2    => Brushes.Beige,
        4    => Brushes.Bisque,
        8    => Brushes.Orange,
        16   => Brushes.OrangeRed,
        32   => Brushes.Red,
        64   => Brushes.DarkRed,
        128  => Brushes.Gold,
        256  => Brushes.Goldenrod,
        512  => Brushes.Khaki,
        1024 => Brushes.LightYellow,
        2048 => Brushes.Yellow,
        _    => Brushes.Gray
    };
    
    public IBrush Foreground => Value <= 4 ? Brushes.Black : Brushes.White;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


}