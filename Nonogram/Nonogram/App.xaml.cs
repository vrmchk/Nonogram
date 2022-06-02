using System;
using System.Windows;
using Nonogram.Views;


namespace Nonogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            menuWindow.GameModeChosen += Game_Mode_Chosen;
        }

        private void Game_Mode_Chosen()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}