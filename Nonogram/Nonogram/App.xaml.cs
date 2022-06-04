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
        private MenuWindow _menuWindow;
        private MainWindow _mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _viewModel = new ViewModel();
            _menuWindow = new MenuWindow(_viewModel);
            _menuWindow.Show();
            _menuWindow.GameModeChosen += Game_Mode_Chosen;
        }

        private void Game_Mode_Chosen()
        {
            _mainWindow = new MainWindow(_viewModel);
            _mainWindow.Show();
        }
    }
}