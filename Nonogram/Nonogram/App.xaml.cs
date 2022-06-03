using System;
using System.Windows;
using Nonogram.ViewModels;
using Nonogram.Views;


namespace Nonogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ViewModel _viewModel;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _viewModel = new ViewModel();
            MenuWindow menuWindow = new MenuWindow(_viewModel);
            menuWindow.Show();
            menuWindow.GameModeChosen += Game_Mode_Chosen;
        }

        private void Game_Mode_Chosen()
        {
            MainWindow mainWindow = new MainWindow(_viewModel);
            mainWindow.Show();
        }
    }
}