namespace P3Ide.ViewModel {

    /// <summary>
    ///     view model for the main window
    /// </summary>
    public class MainViewModel {

        /// <summary>
        ///     view model for docking
        /// </summary>
        public DockManagerViewModel DockManagerViewModel { get; }

        /// <summary>
        ///     view model for the menu
        /// </summary>
        public MenuViewModel MenuViewModel { get; }

        /// <summary>
        ///     create a new view model
        /// </summary>
        public MainViewModel() {
            DockManagerViewModel = new DockManagerViewModel();
            MenuViewModel = new MenuViewModel();

            var fileMenu = new MenuViewModel() { Text = "File" };
            fileMenu.MenuItems.Add(new MenuViewModel() { Text = "Open" });
            MenuViewModel.MenuItems.Add(fileMenu);

            DockManagerViewModel.Documents.Add(new DockWindowViewModel());
            DockManagerViewModel.Documents.Add(new DockWindowViewModel());
            DockManagerViewModel.Documents.Add(new DockWindowViewModel());
            //this.MenuViewModel = new MenuViewModel(documents);

        }
    }
}
