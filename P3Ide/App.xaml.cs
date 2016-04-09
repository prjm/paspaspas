using P3Ide.ViewModel;
using System.Windows;

namespace P3Ide {
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application {

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);


            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            mainWindow.Show();

            var mainViewModel = new MainViewModel();
            mainWindow.DataContext = mainViewModel;
        }

    }
}
