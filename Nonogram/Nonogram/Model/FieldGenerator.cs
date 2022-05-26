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
        _cells.Add(new Cell(GetRandomColor()));
        for (int row = 1; row < Settings.GameGridsCount; row++)
        {
            _cells.Add(new Cell(GetPossiblySimilarBrush(_cells.Last().Color)));
        }
    }

    private CellColor GetRandomColor()
    {
        return _random.Next(0, 2) == 0 ? CellColor.First : CellColor.Second;
    }

    private CellColor GetPossiblySimilarBrush(CellColor color, int probabilityOfSimilarity = 80)
    {
        if (probabilityOfSimilarity is <= 0 or > 100)
            return GetRandomColor();

        if (_random.Next(1, 101) < probabilityOfSimilarity)
            return color;

        else
            return color != CellColor.First ? CellColor.First : CellColor.Second;
    }

    private void FillBrushCounts()
    {
        FillVertical(CellColor.First);
        FillVertical(CellColor.Second);
        FillHorizontal(CellColor.First);
        FillHorizontal(CellColor.Second);
    }

    private void FillHorizontal(CellColor color)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                CellColor cellBrush = Cells[i * _rowColSize + j].Color;

                if (cellBrush == color)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellBrush != color)
                    currentCount = 0;
            }

            _brushCounts.Add(maxCount);
        }
    }

    private void FillVertical(CellColor color)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                CellColor cellBrush = Cells[j * _rowColSize + i].Color;

                if (cellBrush == color)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellBrush != color)
                    currentCount = 0;
            }

            _brushCounts.Add(maxCount);
        }
    }
}