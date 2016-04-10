namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     view model for the main window
    /// </summary>
    public interface IMainViewModel {

        /// <summary>
        ///     view model for the main menu
        /// </summary>
        IMainMenuViewModel MainMenu { get; }

        /// <summary>
        ///     editor capabilities
        /// </summary>
        IEditorCapabilites EditorCapabilities { get; }

    }
}
