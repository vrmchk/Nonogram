using System.Windows.Media;

namespace Nonogram.Model;

internal class FillCellCommand : ICommand
{
    private Cell _cell;
    private CellColor _color;


    public FillCellCommand(Cell cell, CellColor color)
    {
        _cell = cell;
        _color = color;
    }

    public bool WasExecuted { get; private set; }

    public void Execute()
    {
        if (_cell.IsFound || _cell.Color != _color)
        {
            WasExecuted = false;
            return;
        }

        _cell.IsFound = true;
        WasExecuted = true;
    }

    public void Undo() => _cell.IsFound = false;
}