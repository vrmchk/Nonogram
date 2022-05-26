using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nonogram.Model;

internal class FieldPrinter
{
    private Field _field;

    public FieldPrinter(Field field)
    {
        _field = field;
        _field.BrushGrid += Brush_Grid;
    }

    private void Brush_Grid(Grid sender, BrushGridEventArgs e)
    {
        sender.Background = e.BrushOperationTypes switch
        {
            BrushOperationTypes.WithBrush => e.Color == CellColor.First ? Settings.Brush1 : Settings.Brush2,
            BrushOperationTypes.Default => Settings.DefaultBrush,
            BrushOperationTypes.Incorrect => Settings.WrongBrush,
            _ => throw new ArgumentOutOfRangeException(nameof(e))
        };
    }
}