using System;
using System.Windows.Controls;
using System.Windows.Media;
using Nonogram.Enums;

namespace Nonogram.Models;

internal class FieldPrinter
{
    private readonly Field _field;
    
    public FieldPrinter(Field field)
    {
        _field = field;
        _field.ChangeGridColor += Change_Grid_Color;
    }
    
    public static Brush Brush1 { get; } = new SolidColorBrush(Colors.Black);
    public static Brush Brush2 { get; } = new SolidColorBrush(Colors.Gold);
    public static Brush WrongBrush { get; } = new SolidColorBrush(Colors.Red);
    public static Brush DefaultBrush { get; } = new SolidColorBrush(Colors.White);

    private void Change_Grid_Color(Grid sender, ChangeGridColorEventArgs e)
    {
        sender.Background = e.ChangeColorOperationTypes switch
        {
            ChangeColorOperationTypes.WithColor => e.Color == CellColor.First ? Brush1 : Brush2,
            ChangeColorOperationTypes.Default => DefaultBrush,
            ChangeColorOperationTypes.Incorrect => WrongBrush,
            _ => throw new ArgumentOutOfRangeException(nameof(e))
        };
    }
}