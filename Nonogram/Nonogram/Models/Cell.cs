using Nonogram.Enums;


namespace Nonogram.Models;

internal delegate void CellChangedEventHandler(Cell sender);

internal class Cell
{
    private bool _isFound;

    public Cell(CellColor color, int coordinate)
    {
        Color = color;
        Coordinate = coordinate;
        IsFound = false;
    }

    public CellColor Color { get; }
    public int Coordinate { get; }

    public bool IsFound
    {
        get => _isFound;
        set
        {
            _isFound = value;
            CellChanged?.Invoke(this);
        }
    }

    public event CellChangedEventHandler? CellChanged;
}