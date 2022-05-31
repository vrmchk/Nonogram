using System.Windows.Media;

namespace Nonogram.Models;

internal class GiveHintCommand : ICommand
{
    private Cell _cell;

    public GiveHintCommand(Cell cell) => _cell = cell;

    public void Execute() => _cell.IsFound = true;

    public void Undo() => _cell.IsFound = false;
}