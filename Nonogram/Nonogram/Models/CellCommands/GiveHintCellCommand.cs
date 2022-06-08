namespace Nonogram.Models.CellCommands;

/// <summary>
/// Command pattern implemented
/// </summary>
internal class GiveHintCellCommand : ICellCommand
{
    private readonly Cell _cell;

    public GiveHintCellCommand(Cell cell) => _cell = cell;

    public void Execute() => _cell.IsFound = true;

    public Cell Undo()
    {
        _cell.IsFound = false;
        return _cell;
    }
}