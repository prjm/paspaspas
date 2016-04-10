using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace P3Ide.ViewModel.Base {

    /// <summary>
    ///     a simple view model for menus
    /// </summary>
    public class MenuViewModel : IMenuViewModel {

        /// <summary>
        ///     menu items
        /// </summary>
        public ObservableCollection<IMenuViewModel> MenuItems { get; }

        /// <summary>
        ///     menu item text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     menu command
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        ///     create a new menu view model
        /// </summary>
        public MenuViewModel() {
            MenuItems = new ObservableCollection<IMenuViewModel>();
        }

        /// <summary>
        ///     add menu items
        /// </summary>
        /// <param name="items"></param>
        public void AddMenuItems(IEnumerable<IMenuViewModel> items) {
            foreach (var item in items)
                MenuItems.Add(item);
        }

    }
}
