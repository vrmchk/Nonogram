﻿using System;
using System.Windows;
using Nonogram.ViewModels;

namespace Nonogram.Views;

public partial class MenuWindow : Window
{
    private readonly string _rules = "There is numbers on the left, right, top and bottom." +
                                     "Left and top numbers show the biggest block of black cells in this row(column). " +
                                     "Right and bottom numbers point on the biggest block of yellow cells. " +
                                     "There may be several of these blocks. " +
                                     "Press left mouse button to fill the cell with black color "
                                     + "and right mouse button to fill with yellow.";

    public MenuWindow()
    {
        InitializeComponent();
        InitializeUiElementsProperties();
    }
    
    public event Action GameModeChosen;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        GameModeChosen?.Invoke();
        Close();
    }

    private void InitializeUiElementsProperties()
    {
        RulesBlock.Text = _rules;
        ViewModel viewModel = ViewModel.GetInstance();
        ContinueButton.Command = viewModel.LoadExistingGameCommand;
    }
}