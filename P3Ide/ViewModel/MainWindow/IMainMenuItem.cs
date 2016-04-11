namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     interface for main menu items
    /// </summary>
    public interface IMainMenuItem : IMainMenuViewModel {

        /// <summary>
        ///     top level menu category
        /// </summary>
        string TopLevelMenu { get; }

        /// <summary>
        ///     menu prioerty
        /// </summary>
        int Priority { get; }

    }
}
