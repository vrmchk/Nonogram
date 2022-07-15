using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Nonogram.Enums;
using Nonogram.Models.CellCommands;

namespace Nonogram.Models;

internal class Field
{
    private List<Cell> _cells;
    private Stack<ICellCommand> _commandsHistory;
    private int _hintsLeft;
    private readonly Random _random = new();

    public Field(IFieldGenerator generator)
    {
        GenerateNewField(generator);
    }

    public List<int> ColorsCounts { get; private set; }

    public event Action? GameFinished;
    public event EventHandler? CellChanged;

    public void GenerateNewField(IFieldGenerator generator)
    {
        _commandsHistory = new Stack<ICellCommand>();
        _cells = generator.Cells;
        ColorsCounts = generator.ColorsCounts;
        _hintsLeft = generator.HintsLeft;
        _cells.ForEach(c => c.PropertyChanged += Cell_Property_Changed);
        _cells.Where(c => c.State != CellState.NotFound).ToList().ForEach(c => Cell_Property_Changed(c));
    }

    public void Deconstruct(out List<Cell> cells, out List<int> colorsCounts, out int hintsLeft)
    {
        cells = _cells;
        colorsCounts = ColorsCounts;
        hintsLeft = _hintsLeft;
    }

    public bool CanFillCell(int cellIndex) => _cells[cellIndex].State != CellState.Found;

    public void FillCell(int cellIndex, CellColor color)
    {
        FillCellCommand cellCommand = new FillCellCommand(_cells[cellIndex], color);
        cellCommand.Execute();
        _commandsHistory.Push(cellCommand);
    }

    public void GiveHint()
    {
        if (IsSolved())
            throw new InvalidOperationException("Game is already solved");

        if (_hintsLeft-- <= 0)
            throw new InvalidOperationException("No more hints left");

        var notFoundCells = _cells.Where(c => c.State == CellState.NotFound).ToArray();
        var incorrectCells = _cells.Where(c => c.State == CellState.FoundIncorrect).ToArray();
        var cellsToPeekRandom = notFoundCells.Length != 0 ? notFoundCells : incorrectCells;
        Cell randomCell = cellsToPeekRandom[_random.Next(cellsToPeekRandom.Length)];
        GiveHintCellCommand cellCommand = new GiveHintCellCommand(randomCell);
        cellCommand.Execute();
        _commandsHistory.Push(cellCommand);
    }

    public void Undo()
    {
        if (_commandsHistory.Count == 0)
            throw new InvalidOperationException("Nothing to undo");

        ICellCommand cellCommand = _commandsHistory.Pop();
        cellCommand.Undo();
    }

    public void Solve()
    {
        if (IsSolved())
            throw new InvalidOperationException("Game is already solved");

        _cells.Where(c => c.State != CellState.Found).ToList().ForEach(c => FillCell(c.Index, c.Color));
    }

    private bool IsSolved() => _cells.All(cell => cell.State != CellState.NotFound);

    private void Cell_Property_Changed(object? sender, PropertyChangedEventArgs? e = null)
    {
        CellChanged?.Invoke(sender, EventArgs.Empty);
        if (IsSolved())
            GameFinished?.Invoke();
    }
}