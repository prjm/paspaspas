using System;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     view model for the main window
    /// </summary>
    public class MainViewModel : IMainViewModel {

        /// <summary>
        ///     creates a new main view model
        /// </summary>
        /// <param name="mainMenu"></param>
        /// <param name="capabilities">capabilities</param>
        public MainViewModel(IMainMenuViewModel mainMenu, IEditorCapabilites capabilities) {
            MainMenu = mainMenu;
            EditorCapabilities = capabilities;
        }

        /// <summary>
        ///     ide capabilites
        /// </summary>
        public IEditorCapabilites EditorCapabilities { get; }

        /// <summary>
        ///     main menu
        /// </summary>
        public IMainMenuViewModel MainMenu { get; }
    }

}
