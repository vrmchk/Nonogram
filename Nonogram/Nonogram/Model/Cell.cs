using System;
using System.Windows.Media;
using Nonogram.Enums;


namespace Nonogram.Model;

internal delegate void CellChangedHandler(Cell sender);

internal class Cell
{
    private bool _isFound;

    public Cell(CellColor color)
    {
        Color = color;
        IsFound = false;
    }

    public CellColor Color { get; }

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