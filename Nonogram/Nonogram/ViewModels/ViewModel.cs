using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Nonogram.Annotations;
using Nonogram.Commands;
using Nonogram.Enums;
using Nonogram.Models;

namespace Nonogram.ViewModels;

public sealed class ViewModel : INotifyPropertyChanged
{
    #region Fields

    private readonly Field _field;
    private readonly FieldSaver _saver;
    private readonly int _brushesCount = Settings.CellsCount;

    #endregion

    #region Constructors

    public ViewModel()
    {
        _field = new Field(new RandomFieldGenerator());
        _saver = new FieldSaver(_field);
        _field.GameFinished += Game_Finished;
        _field.CellChanged += Cell_Changed;
        FillCellWithColor1Command = new RelayCommand(FillCellWithColor1, CanFillCell);
        FillCellWithColor2Command = new RelayCommand(FillCellWithColor2, CanFillCell);
        SaveCommand = new RelayCommand(Save);
        LoadExistingGameCommand = new RelayCommand(LoadExistingGame);
        NewGameCommand = new RelayCommand(NewGame);
        SolveCommand = new RelayCommand(Solve);
        GiveHintCommand = new RelayCommand(GiveHint);
        UndoCommand = new RelayCommand(Undo);
        FillBrushesByDefault();
        FillColorCounts();
    }

    #endregion

    #region BindingProperties

    //Brushes, binded with _field CellColors
    public List<Brush> Brushes { get; set; }

    //ColorsCounts, binded with _field ColorsCounts
    public List<string> ColorsCounts { get; set; }

    #endregion

    #region Commands

    public ICommand FillCellWithColor1Command { get; }
    public ICommand FillCellWithColor2Command { get; }
    public ICommand LoadExistingGameCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand SolveCommand { get; }
    public ICommand GiveHintCommand { get; }
    public ICommand UndoCommand { get; }

    #endregion

    #region CommandsMethods

    private void FillCellWithColor1(object p) => FillCell(Convert.ToInt32(p), CellColor.First);

    private void FillCellWithColor2(object p) => FillCell(Convert.ToInt32(p), CellColor.Second);

    private void FillCell(int cellIndex, CellColor color) => _field.FillCell(cellIndex, color);

    private bool CanFillCell(object p) => _field.CanFillCell(Convert.ToInt32(p));

    private void Save(object p) => _saver.Save();

    private void NewGame(object p)
    {
        _field.GenerateNewField(new RandomFieldGenerator());
        FillBrushesByDefault();
        FillColorCounts();
    }

    private void Solve(object p)
    {
        try
        {
            _field.Solve();
        }
        catch (Exception ex)
        {
            ShowMessage?.Invoke(ex.Message);
        }
    }

    private void GiveHint(object p)
    {
        try
        {
            _field.GiveHint();
        }
        catch (Exception ex)
        {
            ShowMessage?.Invoke(ex.Message);
        }
    }

    private void Undo(object p)
    {
        try
        {
            _field.Undo();
        }
        catch (Exception ex)
        {
            ShowMessage?.Invoke(ex.Message);
        }
    }

    private void LoadExistingGame(object p)
    {
        try
        {
            IFieldGenerator fieldGenerator = _saver.GetExistingFieldGenerator();
            _field.GenerateNewField(fieldGenerator);
            FillColorCounts();
        }
        catch (Exception ex)
        {
            ShowMessage?.Invoke(ex.Message);
        }
    }

    #endregion

    #region FieldEventHandlers

    private void Game_Finished() => ShowMessage?.Invoke("Well done!!!");

    private void Cell_Changed(object? sender, EventArgs e)
    {
        Cell cell = (Cell) sender!;
        Brush brush = cell.State switch
        {
            CellState.NotFound => Settings.DefaultBrush,
            CellState.FoundIncorrect => Settings.WrongBrush,
            CellState.Found => cell.Color == CellColor.First ? Settings.Brush1 : Settings.Brush2,
            _ => throw new ArgumentOutOfRangeException()
        };
        ChangeBrush(cell.Index, brush);
    }

    #endregion

    #region PrivateMethods

    private void FillBrushesByDefault()
    {
        Brushes = new List<Brush>();
        for (int i = 0; i < _brushesCount; i++)
        {
            Brushes.Add(Settings.DefaultBrush);
        }

        OnPropertyChanged(nameof(Brushes));
    }

    private void FillColorCounts()
    {
        ColorsCounts = _field.ColorsCounts.Select(item => item.ToString()).ToList();
        OnPropertyChanged(nameof(ColorsCounts));
    }

    private void ChangeBrush(int brushIndex, Brush brush)
    {
        if (brushIndex >= _brushesCount || brushIndex < 0)
            throw new IndexOutOfRangeException(nameof(brushIndex));

        Brushes[brushIndex] = brush;
        OnPropertyChanged(nameof(Brushes));
    }

    #endregion

    #region EventsAndInvocators

    public event Action<string>? ShowMessage;
    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}