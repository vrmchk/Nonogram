using System;
using System.Windows.Media;

namespace Nonogram.Model;

internal class BrushGridEventArgs : EventArgs
{
    public BrushGridEventArgs(BrushOperationTypes brushOperationTypes, Brush? brush = null)
    {
        Brush = brush;
        BrushOperationTypes = brushOperationTypes;
    }

    public Brush? Brush { get; }
    public BrushOperationTypes BrushOperationTypes { get; }
}

internal enum BrushOperationTypes
{
    WithBrush,
    Default,
    Incorrect
}