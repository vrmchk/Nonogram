using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Nonogram.ViewModels;


namespace Nonogram.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;
        private readonly List<Grid> _gameGrids;
        private readonly List<TextBlock> _numberBlocks;

        public MainWindow(ViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _gameGrids = GameField.GetVisualChildren<Grid>().Where(g => g.Tag?.ToString() == "GameGrid").ToList();
            _numberBlocks = GameField.GetVisualChildren<TextBlock>().Where(g => g.Tag?.ToString() == "NumberBlock").ToList();
            InitializeNumberBlocksBindings();
            InitializeGameGridsBindings();
            InitializeButtonsCommands();
            InitializeGameGridsCommands();
        }

        private void InitializeNumberBlocksBindings()
        {
            int index = 0;
            foreach (TextBlock block in _numberBlocks)
            {
                Binding binding = new Binding
                {
                    Source = _viewModel,
                    Path = new PropertyPath($"ColorsCounts[{index++}]"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(block, TextBlock.TextProperty, binding);
            }
        }

        private void InitializeGameGridsBindings()
        {
            int index = 0;
            foreach (Grid grid in _gameGrids)
            {
                Binding binding = new Binding
                {
                    Source = _viewModel,
                    Path = new PropertyPath($"Brushes[{index++}]"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(grid, Grid.BackgroundProperty, binding);
            }
        }

        private void InitializeButtonsCommands()
        {
            SaveButton.Command = _viewModel.SaveCommand;
            NewGameButton.Command = _viewModel.NewGameCommand;
            SolveButton.Command = _viewModel.SolveCommand;
            HintButton.Command = _viewModel.GiveHintCommand;
            UndoButton.Command = _viewModel.UndoCommand;
        }

        private void InitializeGameGridsCommands()
        {
            for (int i = 0; i < _gameGrids.Count; i++)
            {
                InputBindingCollection inputBindings = _gameGrids[i].InputBindings;
                inputBindings.Add(new MouseBinding
                {
                    MouseAction = MouseAction.LeftClick,
                    Command = _viewModel.FillCellWithColor1Command,
                    CommandParameter = i
                });
                inputBindings.Add(new MouseBinding
                {
                    MouseAction = MouseAction.RightClick,
                    Command = _viewModel.FillCellWithColor2Command,
                    CommandParameter = i
                });
            }
        }
    }
}