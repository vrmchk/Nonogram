using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Nonogram.Model;

internal delegate void GameFinishedHandler();

internal delegate void BrushGridHandler(Grid sender, BrushGridEventArgs e);

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
    public event BrushGridHandler BrushGrid;

    public void GenerateNewField()
    {
        FieldGenerator generator = new FieldGenerator();
        _cells = generator.Cells.ToList();
        _commandsHistory = new Stack<ICommand>();
        _hintsLeft = 3;
        
        _gameGrids.ForEach(g => g.Background = Settings.DefaultBrush);
        _numberBlocks.ForEach(b => b.Text = generator.BrushCounts[_numberBlocks.IndexOf(b)].ToString());
        _cells.ForEach(c => c.CellChanged += Cell_Changed);
    }

    private void Cell_Changed(Cell sender)
    {
        Grid grid = _gameGrids[_cells.IndexOf(sender)];
        if (sender.IsFound)
            BrushGrid?.Invoke(grid, new BrushGridEventArgs(BrushOperationTypes.WithBrush, sender.Brush));
        else
            BrushGrid?.Invoke(grid, new BrushGridEventArgs(BrushOperationTypes.Default));
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

    public void Solve() => _cells.ForEach(c => c.IsFound = true);

    private bool IsSolved() => _cells.All(c => c.IsFound);

    private bool FillCell(Cell cell, Brush brush)
    {
        FillCellCommand command = new FillCellCommand(cell, brush);
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
        GridMouseButtonUp((Grid) sender, e, Settings.Brush1);
    }

    private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        GridMouseButtonUp((Grid) sender, e, Settings.Brush2);
    }

    private void GridMouseButtonUp(Grid sender, MouseButtonEventArgs e, Brush brush)
    {
        Cell cell = _cells[_gameGrids.IndexOf(sender)];
        if (cell.IsFound) return;
        if (FillCell(cell, brush)) return;
        BrushGrid?.Invoke(sender, new BrushGridEventArgs(BrushOperationTypes.Incorrect));
    }
}