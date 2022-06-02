using System.Windows.Media;

namespace Nonogram.Models.CellCommans;

internal interface ICellCommand
{
    public void Execute();
    public Cell Undo();
}