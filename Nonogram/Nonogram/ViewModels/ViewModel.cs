using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Nonogram.Annotations;
using Nonogram.Commands;
using Nonogram.Enums;
using Nonogram.Models;

namespace Nonogram.ViewModels;

public sealed class ViewModel : INotifyPropertyChanged
{
    private static ViewModel? _instance;
    private readonly Field _field;
    private readonly FieldSaver _saver;
    private readonly int _brushesCount = 225;

    private ViewModel()
    {
        MessageBox.Show("object created");
        _field = new Field();
        _saver = new FieldSaver(_field);
        _field.GameFinished += Game_Finished;
        _field.CellChangedOnField += Cell_Changed_On_Field;
        FillCellWithFirstColorCommand = new RelayCommand(FillCellWithFirstColor, CanDoMouseClick);
        FillCellWithSecondColorCommand = new RelayCommand(FillCellWithSecondColor, CanDoMouseClick);
        SaveCommand = new RelayCommand(Save);
        LoadExistingGameCommand = new RelayCommand(LoadExistingGame);
        NewGameCommand = new RelayCommand(NewGame);
        SolveCommand = new RelayCommand(Solve);
        GiveHintCommand = new RelayCommand(GiveHint);
        UndoCommand = new RelayCommand(Undo);
        FillBrushesByDefault();
        FillNumberBlocksText();
    }

    public List<Brush> Brushes { get; set; }
    public List<string> ColorsCounts { get; set; }

    #region Commands

    public ICommand FillCellWithFirstColorCommand { get; }
    public ICommand FillCellWithSecondColorCommand { get; }
    public ICommand LoadExistingGameCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand SolveCommand { get; }
    public ICommand GiveHintCommand { get; }
    public ICommand UndoCommand { get; }

    #endregion


    public static ViewModel GetInstance()
    {
        return _instance ??= new ViewModel();
    }
    
    private void Cell_Changed_On_Field(Cell sender)
    {
        ChangeBrush(sender.Coordinate, sender.Color == CellColor.First ? Printer.Brush1 : Printer.Brush2);
        OnPropertyChanged(nameof(Brushes));
    }

    private void Game_Finished() => MessageBox.Show("Well done!!!");


    private void FillCellWithFirstColor(object p)
    {
        FillCellWithColor(Convert.ToInt32(p), CellColor.First);
    }

    private void FillCellWithSecondColor(object p)
    {
        FillCellWithColor(Convert.ToInt32(p), CellColor.Second);
    }

    private void FillCellWithColor(int cellIndex, CellColor color)
    {
        bool cellFilled = _field.FillCell(cellIndex, color);
        if (!cellFilled)
        {
            ChangeBrush(cellIndex, Printer.WrongBrush);
        }

        OnPropertyChanged(nameof(Brushes));
    }

    private bool CanDoMouseClick(object p) => _field.CanFillCell(Convert.ToInt32(p));

    private void Save(object p) => _saver.Save();

    private void NewGame(object p)
    {
        _field.GenerateNewField();
        FillNumberBlocksText();
        FillBrushesByDefault();
    }

    private void Solve(object p) => _field.Solve();

    private void GiveHint(object p)
    {
        try
        {
            _field.GiveHint();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void Undo(object p)
    {
        try
        {
            int cellIndex = _field.Undo();
            ChangeBrush(cellIndex, Printer.DefaultBrush);
            OnPropertyChanged(nameof(Brushes));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void LoadExistingGame(object p)
    {
        try
        {
            _saver.LoadExistingGame();
            FillNumberBlocksText();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void FillBrushesByDefault()
    {
        Brushes = new List<Brush>();
        for (int i = 0; i < _brushesCount; i++)
        {
            Brushes.Add(Printer.DefaultBrush);
        }

        OnPropertyChanged(nameof(Brushes));
    }

    private void FillNumberBlocksText()
    {
        ColorsCounts = _field.ColorsCounts.Select(i => i.ToString()).ToList();
        OnPropertyChanged(nameof(ColorsCounts));
    }

    private void ChangeBrush(int brushIndex, Brush brush)
    {
        if (brushIndex >= _brushesCount || brushIndex < 0)
            throw new IndexOutOfRangeException(nameof(brushIndex));

        Brushes[brushIndex] = brush;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}