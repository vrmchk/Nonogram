using System.Windows.Media;

namespace Nonogram.Models;

internal interface ICommand
{
    public void Execute();
    public void Undo();
}