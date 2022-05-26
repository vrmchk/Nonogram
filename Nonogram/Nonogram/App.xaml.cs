using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Nonogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            PreviewWindow previewWindow = new PreviewWindow();
            previewWindow.Show();
            previewWindow.GameModeChosen += Game_Mode_Chosen;
        }

        private void Game_Mode_Chosen(bool newGame)
        {
            MainWindow mainWindow = new MainWindow(newGame);
            mainWindow.Show();
        }
    }
}