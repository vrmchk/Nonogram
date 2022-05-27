using System;
using System.Windows.Media;

namespace Nonogram.Model;

internal class ChangeGridColorEventArgs : EventArgs
{
    public ChangeGridColorEventArgs(ChangeColorOperationTypes changeColorOperationTypes)
    {
        ChangeColorOperationTypes = changeColorOperationTypes;
        Color = null;
    }
    
    public ChangeGridColorEventArgs(ChangeColorOperationTypes changeColorOperationTypes, CellColor color) : this(changeColorOperationTypes)
    {
        Color = color;
    }

    public CellColor? Color { get; }
    public ChangeColorOperationTypes ChangeColorOperationTypes { get; }
}

internal enum ChangeColorOperationTypes
{
    WithColor,
    Default,
    Incorrect
}