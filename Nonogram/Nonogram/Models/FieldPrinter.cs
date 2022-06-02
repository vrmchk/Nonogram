using System;
using System.Windows.Controls;
using System.Windows.Media;
using Nonogram.Enums;

namespace Nonogram.Models;

internal static class FieldPrinter
{
    public static Brush Brush1 { get; } = new SolidColorBrush(Colors.Black);
    public static Brush Brush2 { get; } = new SolidColorBrush(Colors.Gold);
    public static Brush WrongBrush { get; } = new SolidColorBrush(Colors.Red);
    public static Brush DefaultBrush { get; } = new SolidColorBrush(Colors.White);
}