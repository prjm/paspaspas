using P3Ide.ViewModel.Base;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     model for the main menu
    /// </summary>
    public class MainMenuViewModel : MenuViewModel, IMainMenuViewModel {

        /// <summary>
        ///     create a new main menu
        /// </summary>
        public MainMenuViewModel(IEditorCapabilites capabilities) {
            var fileMenu = new MenuViewModel() { Text = "File" };
            fileMenu.AddMenuItems(new[] {
                new MenuViewModel() { Text = "New" },
                new MenuViewModel() { Text = "Open", Command = new FileOpenCommand(capabilities) },
                new MenuViewModel() { Text = "Add" },
                new MenuViewModel() { Text = "Close" },
            });

            AddMenuItems(new[] {
                fileMenu
            });
        }

    }
}
