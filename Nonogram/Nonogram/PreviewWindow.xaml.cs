using System.Windows;

namespace Nonogram;

public delegate void GameModeChosenHandler(bool newGame);

public partial class PreviewWindow : Window
{
    private readonly string _rules = "There is numbers on the left, right, top and bottom." +
                                     "Left and top numbers show the biggest block of black cells in this row(column). " +
                                     "Right and bottom numbers point on the biggest block of yellow cells. " +
                                     "There may be several of these blocks. " +
                                     "Press left mouse button to fill the cell with black color "
                                     + "and right mouse button to fill with yellow.";

    public PreviewWindow()
    {
        InitializeComponent();
        RulesBlock.Text = _rules;
    }


    public event GameModeChosenHandler GameModeChosen;

    private void NewGame_Button_Click(object sender, RoutedEventArgs e)
    {
        GameModeChosen?.Invoke(newGame: true);
        Close();
    }

    private void Continue_Button_Click(object sender, RoutedEventArgs e)
    {
        GameModeChosen?.Invoke(newGame: false);
        Close();
    }
}