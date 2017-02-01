using System.Windows;
using Part2Project_GUI.ViewModel;

namespace Part2Project_GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            // Create the ViewModel to which 
            // the main window binds. 
            var viewModel = new MainWindowViewModel();
            // When the ViewModel asks to be closed, 
            // close the window. 
            viewModel.RequestClose += delegate { window.Close(); };
            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree. 
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
