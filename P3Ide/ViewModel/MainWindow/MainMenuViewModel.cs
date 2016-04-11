using P3Ide.ViewModel.Base;
using System.Collections.Generic;
using System.Linq;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     model for the main menu
    /// </summary>
    public class MainMenuViewModel : MenuViewModel, IMainMenuViewModel {

        /// <summary>
        ///     create a new main menu
        /// </summary>
        public MainMenuViewModel(IEnumerable<IMainMenuItem> menuItems) {

            foreach (var mainMenu in menuItems.GroupBy(t => t.TopLevelMenu)) {
                var topLevelMenu = new MenuViewModel() { Text = mainMenu.Key };

                foreach (var menuEntry in mainMenu.OrderBy(t => t.Priority)) {
                    topLevelMenu.MenuItems.Add(menuEntry);
                }

                MenuItems.Add(topLevelMenu);
            }
        }

    }
}
