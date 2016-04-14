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
        /// <param name="workspace">editot workspace</param>
        public MainViewModel(IMainMenuViewModel mainMenu, IEditorCapabilites capabilities, IEditorWorkspace workspace) {
            MainMenu = mainMenu;
            EditorCapabilities = capabilities;
            Workspace = workspace;
        }

        /// <summary>
        ///     ide capabilites
        /// </summary>
        public IEditorCapabilites EditorCapabilities { get; }

        /// <summary>
        ///     main menu
        /// </summary>
        public IMainMenuViewModel MainMenu { get; }

        /// <summary>
        ///     workspace
        /// </summary>
        public IEditorWorkspace Workspace { get; }
    }

}
