using Nonogram.Enums;

namespace Nonogram.Models.CellCommands;

internal class FillCellCommand : ICellCommand
{
    private readonly Cell _cell;
    private readonly CellColor _color;


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
    
    public Cell Undo()
    {
        _cell.IsFound = false;
        return _cell;
    }
}