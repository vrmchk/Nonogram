using System;
using System.Collections.Generic;
using System.Linq;
using Nonogram.Enums;
using Nonogram.Models.CellCommans;

namespace Nonogram.Models;

internal delegate void GameFinishedHandler();

internal delegate void CellChangedOnFieldHandler(Cell sender);

internal class Field
{
    private List<Cell> _cells;
    private Stack<ICellCommand> _commandsHistory;
    private int _hintsLeft;
    private readonly Random _random;

    public Field()
    {
        _random = new Random();
        GenerateNewField();
    }

    public List<int> ColorsCounts { get; private set; }

    public event GameFinishedHandler GameFinished;
    public event CellChangedOnFieldHandler CellChangedOnField;

    public void GenerateNewField()
    {
        _hintsLeft = 3;
        _commandsHistory = new Stack<ICellCommand>();
        FieldGenerator generator = new FieldGenerator();
        _cells = generator.Cells;
        ColorsCounts = generator.ColorsCounts;
        _cells.ForEach(c => c.CellChanged += Cell_Changed);
    }

    public void Deconstruct(out List<Cell> cells, out List<int> colorsCounts, out int hintsLeft)
    {
        cells = _cells;
        colorsCounts = ColorsCounts;
        hintsLeft = _hintsLeft;
    }

    public void LoadExistingGame(List<Cell> cells, List<int> colorsCounts, int hintsLeft)
    {
        _cells = cells;
        ColorsCounts = colorsCounts;
        _hintsLeft = hintsLeft;
        _cells.ForEach(c => c.CellChanged += Cell_Changed);
        _cells.Where(c => c.IsFound).ToList().ForEach(Cell_Changed);
    }

    public bool FillCell(int cellIndex, CellColor color)
    {
        FillCellCommand cellCommand = new FillCellCommand(_cells[cellIndex], color);
        cellCommand.Execute();
        _commandsHistory.Push(cellCommand);
        return cellCommand.WasExecuted;
    }

    public bool CanFillCell(int cellIndex) => !_cells[cellIndex].IsFound;

    public void GiveHint()
    {
        if (_hintsLeft-- <= 0)
            throw new InvalidOperationException("No more hints left");

        var notFound = _cells.Where(c => !c.IsFound).ToList();
        Cell randomCell = notFound[_random.Next(notFound.Count)];
        GiveHintCellCommand cellCommand = new GiveHintCellCommand(randomCell);
        cellCommand.Execute();
        _commandsHistory.Push(cellCommand);
    }

    public int Undo()
    {
        if (_commandsHistory.Count == 0)
            throw new InvalidOperationException("Nothing to undo");

        ICellCommand cellCommand = _commandsHistory.Pop();
        Cell cell = cellCommand.Undo();
        return cell.Coordinate;
    }

    public void Solve()
    {
        if (!IsSolved())
            _cells.ForEach(c => c.IsFound = true);
    }

    private bool IsSolved() => _cells.All(c => c.IsFound);

    private void Cell_Changed(Cell sender)
    {
        CellChangedOnField?.Invoke(sender);
        if (IsSolved())
            GameFinished?.Invoke();
    }
}