namespace Nonogram.Models.CellCommands;

/// <summary>
/// Command pattern interface 
/// </summary>
internal interface ICellCommand
{
    public void Execute();
    public void Undo();
}