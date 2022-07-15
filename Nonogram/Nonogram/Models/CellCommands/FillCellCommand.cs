using Nonogram.Enums;

namespace Nonogram.Models.CellCommands;

/// <summary>
/// Command pattern implemented
/// </summary>
internal class FillCellCommand : ICellCommand
{
    private readonly Cell _cell;
    private readonly CellColor _color;
    private readonly CellState _previousState;

    public FillCellCommand(Cell cell, CellColor color)
    {
        _cell = cell;
        _color = color;
        _previousState = _cell.State;
    }

    public void Execute()
    {
        if (_cell.State == CellState.Found) return;
        _cell.State = _cell.Color == _color ? CellState.Found : CellState.FoundIncorrect;
    }

    public void Undo() => _cell.State = _previousState;
}