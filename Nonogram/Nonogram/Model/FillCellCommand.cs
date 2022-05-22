using System.Windows.Media;

namespace Nonogram.Model;

internal class FillCellCommand : ICommand
{
    private Cell _cell;
    private Brush _brush;
    
    
    public FillCellCommand(Cell cell, Brush brush)
    {
        _cell = cell;
        _brush = brush;
    }

    public bool WasExecuted { get; private set; }

    public void Execute()
    {
        if (_cell.IsFound || _cell.Brush != _brush)
        {
            WasExecuted = false;
            return;
        }

        _cell.IsFound = true;
        WasExecuted = true;
    }

    public void Undo() => _cell.IsFound = false;
}