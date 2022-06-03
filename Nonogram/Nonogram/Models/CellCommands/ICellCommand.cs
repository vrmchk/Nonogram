namespace Nonogram.Models.CellCommands;

internal interface ICellCommand
{
    public void Execute();
    public Cell Undo();
}