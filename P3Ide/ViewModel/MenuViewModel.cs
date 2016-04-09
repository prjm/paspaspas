using System.Collections.ObjectModel;

namespace P3Ide.ViewModel {

    /// <summary>
    ///     a simple view model for menus
    /// </summary>
    public class MenuViewModel {

        /// <summary>
        ///     menu items
        /// </summary>
        public ObservableCollection<MenuViewModel> MenuItems { get; }

        /// <summary>
        ///     menu item text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     create a new menu view model
        /// </summary>
        public MenuViewModel() {
            MenuItems = new ObservableCollection<MenuViewModel>();
        }




    }
}
