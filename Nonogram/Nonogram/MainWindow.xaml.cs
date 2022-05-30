using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nonogram.Extensions;
using Nonogram.Model;


namespace Nonogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Field _field;
        private FieldPrinter _printer;
        private FieldSaver _saver;

        public MainWindow(bool newGame = true)
        {
            InitializeComponent();
            var gameGrids
                = Field.GetVisualChildren<Grid>().Where(g => g.Tag?.ToString() == "GameGrid");
            var numberBlocks =
                Field.GetVisualChildren<TextBlock>().Where(b => b.Tag?.ToString() == "NumberBlock");
            _field = new Field(gameGrids, numberBlocks);
            _printer = new FieldPrinter(_field);
            _saver = new FieldSaver(_field);
            _field.GameFinished += Game_Finished;
            if (!newGame) LoadExistingGame();
        }

        private void Game_Finished()
        {
            MessageBox.Show("Well done!!!");
        }

        private void LoadExistingGame()
        {
            try
            {
                _saver.LoadExistingGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            _saver.Save();
        }

        private void NewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            _field.GenerateNewField();
        }

        private void Solve_Button_Click(object sender, RoutedEventArgs e)
        {
            _field.Solve();
        }

        private void Hint_Button_Click(object sender, RoutedEventArgs e)
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

        private void Undo_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _field.Undo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}