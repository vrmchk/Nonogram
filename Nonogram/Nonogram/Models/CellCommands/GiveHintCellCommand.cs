using System.Windows.Media;

namespace Nonogram.Models.CellCommans;

internal class GiveHintCellCommand : ICellCommand
{
    private Cell _cell;

    public GiveHintCellCommand(Cell cell) => _cell = cell;

    public void Execute() => _cell.IsFound = true;

    public Cell Undo()
    {
        _cell.IsFound = false;
        return _cell;
    }
}