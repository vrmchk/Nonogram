using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Nonogram.Model;

internal class FieldGenerator
{
    private Random _random;
    private int _cellsCount;
    private int _rowColSize;

    public FieldGenerator()
    {
        _cellsCount = 225;
        _rowColSize = Convert.ToInt32(Math.Sqrt(_cellsCount));
        _random = new Random();
        Cells = new List<Cell>(_cellsCount);
        ColorCounts = new List<int>(_rowColSize * 4);
        FillCells();
        FillColorCounts();
    }

    public List<Cell> Cells { get; }
    public List<int> ColorCounts { get; }

    private void FillCells()
    {
        Cells.Add(new Cell(GetRandomColor()));
        for (int i = 1; i < _cellsCount; i++)
        {
            Cells.Add(new Cell(GetPossiblySimilarColor(Cells.Last().Color)));
        }
    }

    private CellColor GetRandomColor()
    {
        return _random.Next(0, 2) == 0 ? CellColor.First : CellColor.Second;
    }

    private CellColor GetPossiblySimilarColor(CellColor color, int probabilityOfSimilarity = 80)
    {
        if (probabilityOfSimilarity is <= 0 or > 100)
            return GetRandomColor();

        if (_random.Next(1, 101) < probabilityOfSimilarity)
            return color;

        else
            return color != CellColor.First ? CellColor.First : CellColor.Second;
    }

    private void FillColorCounts()
    {
        FillColumnCounts(CellColor.First);
        FillColumnCounts(CellColor.Second);
        FillRowCounts(CellColor.First);
        FillRowCounts(CellColor.Second);
    }

    private void FillRowCounts(CellColor color)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                CellColor cellColor = Cells[i * _rowColSize + j].Color;

                if (cellColor == color)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellColor != color)
                    currentCount = 0;
            }

            ColorCounts.Add(maxCount);
        }
    }

    private void FillColumnCounts(CellColor color)
    {
        for (int i = 0; i < _rowColSize; i++)
        {
            int currentCount = 0;
            int maxCount = 0;
            for (int j = 0; j < _rowColSize; j++)
            {
                CellColor cellColor = Cells[j * _rowColSize + i].Color;

                if (cellColor == color)
                    currentCount++;

                if (currentCount > maxCount)
                    maxCount = currentCount;

                if (cellColor != color)
                    currentCount = 0;
            }

            ColorCounts.Add(maxCount);
        }
    }
}