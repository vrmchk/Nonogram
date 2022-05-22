using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Nonogram.Model;

internal class FieldGenerator
{
    private List<Cell> _cells;
    private List<int> _brushCounts;
    private Random _random;
    private int _rowColSize;

    public FieldGenerator()
    {
        _random = new Random();
        _cells = new List<Cell>();
        _brushCounts = new List<int>();
        _rowColSize = Convert.ToInt32(Math.Sqrt(Settings.GameGridsCount));
        FillCells();
        FillBrushCounts();
    }

    public Cell[] Cells => _cells.ToArray();
    public int[] BrushCounts => _brushCounts.ToArray();

    private void FillCells()
    {
        _cells.Add(new Cell(GetRandomBrush()));
        for (int row = 1; row < Settings.GameGridsCount; row++)
        {
            _cells.Add(new Cell(GetPossiblySimilarBrush(_cells.Last().Brush)));
        }
    }

    private Brush GetRandomBrush()
    {
        return _random.Next(0, 2) == 0 ? Settings.Brush1 : Settings.Brush2;
    }

    private Brush GetPossiblySimilarBrush(Brush brush, int probabilityOfSimilarity = 70)
    {
        if (probabilityOfSimilarity is <= 0 or > 100)
            return GetRandomBrush();

        if (_random.Next(1, 101) < probabilityOfSimilarity)
            return brush;

        else
            return brush != Settings.Brush1 ? Settings.Brush1 : Settings.Brush2;
    }

    private void FillBrushCounts()
    {
        FillVertical(Settings.Brush1);
        FillVertical(Settings.Brush2);
        FillHorizontal(Settings.Brush1);
        FillHorizontal(Settings.Brush2);
    }

    private void FillHorizontal(Brush brush)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                Brush cellBrush = Cells[i * _rowColSize + j].Brush;

                if (cellBrush == brush)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellBrush != brush)
                    currentCount = 0;
            }

            _brushCounts.Add(maxCount);
        }
    }

    private void FillVertical(Brush brush)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                Brush cellBrush = Cells[j * _rowColSize + i].Brush;

                if (cellBrush == brush)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellBrush != brush)
                    currentCount = 0;
            }

            _brushCounts.Add(maxCount);
        }
    }
}