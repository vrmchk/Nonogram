using System;
using System.Windows.Media;

namespace Nonogram.Model;

internal delegate void CellChangedHandler(Cell sender);

internal class Cell
{
    private readonly Brush _brush;
    private bool _isFound;

    public Cell(Brush brush)
    {
        Brush = brush;
        IsFound = false;
    }

    public Brush Brush
    {
        get => _brush;
        private init
        {
            if (value != Settings.Brush1 && value != Settings.Brush2) throw new ArgumentException(nameof(value));
            if (_isFound) throw new InvalidOperationException(nameof(value));
            _brush = value;
        }
    }

    public bool IsFound
    {
        get => _isFound;
        set
        {
            _isFound = value;
            CellChanged?.Invoke(this);
        }
    }

    public event CellChangedHandler CellChanged;
}