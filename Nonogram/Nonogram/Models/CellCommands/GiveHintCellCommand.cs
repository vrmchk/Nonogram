using Nonogram.Enums;

namespace Nonogram.Models.CellCommands;

/// <summary>
/// Command pattern implemented
/// </summary>
internal class GiveHintCellCommand : ICellCommand
{
    private readonly Cell _cell;
    private readonly CellState _previousState;

    public GiveHintCellCommand(Cell cell)
    {
        _cell = cell;
        _previousState = _cell.State;
    }

    public void Execute() => _cell.State = CellState.Found;
    public void Undo() => _cell.State = _previousState;
}