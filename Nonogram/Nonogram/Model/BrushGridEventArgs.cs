using System;
using System.Windows.Media;

namespace Nonogram.Model;

internal class BrushGridEventArgs : EventArgs
{
    public BrushGridEventArgs(BrushOperationTypes brushOperationTypes)
    {
        BrushOperationTypes = brushOperationTypes;
        Color = null;
    }
    
    public BrushGridEventArgs(BrushOperationTypes brushOperationTypes, CellColor color) : this(brushOperationTypes)
    {
        Color = color;
    }

    public CellColor? Color { get; }
    public BrushOperationTypes BrushOperationTypes { get; }
}

internal enum BrushOperationTypes
{
    WithBrush,
    Default,
    Incorrect
}