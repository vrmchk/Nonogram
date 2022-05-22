using System.Windows.Media;

namespace Nonogram.Model;

internal static class Settings
{
    public static Brush Brush1 { get; } = new SolidColorBrush(Colors.Black);
    public static Brush Brush2 { get; } = new SolidColorBrush(Colors.Gold);
    public static Brush WrongBrush { get; } = new SolidColorBrush(Colors.Red);
    public static Brush DefaultBrush { get; } = new SolidColorBrush(Colors.White);
    public static int GameGridsCount => 225;
}