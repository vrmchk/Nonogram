using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Nonogram.Model;

public static class Extensions
{
    public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject? depObj) where T : DependencyObject
    {
        if (depObj == null) yield break;
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
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
}