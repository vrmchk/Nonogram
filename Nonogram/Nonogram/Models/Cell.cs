using System.ComponentModel;
using System.Runtime.CompilerServices;
using Nonogram.Annotations;
using Nonogram.Enums;

namespace Nonogram.Models;

internal class Cell : INotifyPropertyChanged
{
    private CellState _state;

    public Cell(CellColor color, int index)
    {
        Color = color;
        Index = index;
        State = CellState.NotFound;
    }

    public CellColor Color { get; }
    public int Index { get; }

    public CellState State
    {
        get => _state;
        set
        {
            _state = value;
            OnPropertyChanged(nameof(State));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}