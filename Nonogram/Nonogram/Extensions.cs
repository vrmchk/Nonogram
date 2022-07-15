using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Nonogram.Models;

namespace Nonogram;

internal static class Extensions
{
    public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject? depObj) where T : DependencyObject
    {
        if (depObj == null) yield break;
        int childrenCount = VisualTreeHelper.GetChildrenCount(depObj);
        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
            if (child is T t)
                yield return t;

            foreach (T childOfChild in GetVisualChildren<T>(child))
            {
                yield return childOfChild;
            }
        }
    }

    public static SerializableField AsSerializable(this Field source)
    {
        var (cells, colorsCounts, hintsLeft) = source;
        return new SerializableField(cells, colorsCounts, hintsLeft);
    }

}