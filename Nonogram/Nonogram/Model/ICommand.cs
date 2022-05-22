using System.Windows.Media;

namespace Nonogram.Model;

internal interface ICommand
{
    public void Execute();
    public void Undo();
}