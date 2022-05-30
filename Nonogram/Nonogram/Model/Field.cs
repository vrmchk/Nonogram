using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Nonogram.Enums;

namespace Nonogram.Model;

internal delegate void GameFinishedHandler();

internal delegate void ChangeGridColorHandler(Grid sender, ChangeGridColorEventArgs e);

internal class Field
{
    private List<Cell> _cells;
    private List<Grid> _gameGrids;
    private List<TextBlock> _numberBlocks;
    private Stack<ICommand> _commandsHistory;
    private int _hintsLeft;
    private Random _random;

    public Field(IEnumerable<Grid> gameGrids, IEnumerable<TextBlock> numberBlocks)
    {
        _gameGrids = gameGrids.ToList();
        _numberBlocks = numberBlocks.ToList();
        _random = new Random();

        foreach (Grid grid in _gameGrids)
        {
            grid.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
            grid.MouseRightButtonUp += Grid_MouseRightButtonUp;
        }

        GenerateNewField();
    }

    public event GameFinishedHandler GameFinished;
    public event ChangeGridColorHandler ChangeGridColor;

    public void GenerateNewField()
    {
        FieldGenerator generator = new FieldGenerator();
        _cells = generator.Cells;
        _commandsHistory = new Stack<ICommand>();
        _hintsLeft = 3;

        _gameGrids.ForEach(g =>
            ChangeGridColor?.Invoke(g, new ChangeGridColorEventArgs(ChangeColorOperationTypes.Default)));
        _numberBlocks.ForEach(b => b.Text = generator.ColorCounts[_numberBlocks.IndexOf(b)].ToString());
        _cells.ForEach(c => c.CellChanged += Cell_Changed);
    }

    public void Deconstruct(out List<Cell> cells, out List<string> blockContent, out int hintsLeft)
    {
        cells = _cells;
        blockContent = _numberBlocks.Select(b => b.Text).ToList();
        hintsLeft = _hintsLeft;
    }

    public void LoadExistingGame(List<Cell> cells, List<string> blocksContent, int hintsLeft)
    {
        _cells = cells;
        _numberBlocks.ForEach(b => b.Text = blocksContent[_numberBlocks.IndexOf(b)]);
        _hintsLeft = hintsLeft;
        _cells.ForEach(c => c.CellChanged += Cell_Changed);
        var foundCells = _cells.Where(c => c.IsFound).ToList();
        foundCells.ForEach(Cell_Changed);
    }

    private void Cell_Changed(Cell sender)
    {
        Grid grid = _gameGrids[_cells.IndexOf(sender)];
        if (sender.IsFound)
            ChangeGridColor?.Invoke(grid,
                new ChangeGridColorEventArgs(ChangeColorOperationTypes.WithColor, sender.Color));
        else
            ChangeGridColor?.Invoke(grid, new ChangeGridColorEventArgs(ChangeColorOperationTypes.Default));
        if (IsSolved())
            GameFinished?.Invoke();
    }

    public void GiveHint()
    {
        if (_hintsLeft-- <= 0)
            throw new InvalidOperationException("No more hints left");

        var notFound = _cells.Where(c => !c.IsFound).ToList();
        Cell randomCell = notFound[_random.Next(notFound.Count)];
        GiveHintCommand command = new GiveHintCommand(randomCell);
        command.Execute();
        _commandsHistory.Push(command);
    }

    public void Solve()
    {   
        if (!IsSolved())
            _cells.ForEach(c => c.IsFound = true);
    }

    private bool IsSolved() => _cells.All(c => c.IsFound);

    private bool FillCell(Cell cell, CellColor color)
    {
        FillCellCommand command = new FillCellCommand(cell, color);
        command.Execute();
        _commandsHistory.Push(command);
        return command.WasExecuted;
    }

    public void Undo()
    {
        if (_commandsHistory.Count == 0)
            throw new InvalidOperationException("Nothing to undo");

        ICommand command = _commandsHistory.Pop();
        command.Undo();
    }

    private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        GridMouseButtonUp((Grid) sender, e, CellColor.First);
    }

    private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        GridMouseButtonUp((Grid) sender, e, CellColor.Second);
    }

    private void GridMouseButtonUp(Grid sender, MouseButtonEventArgs e, CellColor color)
    {
        Cell cell = _cells[_gameGrids.IndexOf(sender)];
        if (cell.IsFound) return;
        bool wasFilled = FillCell(cell, color);
        if (!wasFilled) 
            ChangeGridColor?.Invoke(sender, new ChangeGridColorEventArgs(ChangeColorOperationTypes.Incorrect));
    }
}