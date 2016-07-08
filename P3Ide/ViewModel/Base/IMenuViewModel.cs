﻿using System.Collections.ObjectModel;
using System.Windows.Input;

namespace P3Ide.ViewModel.Base {

    /// <summary>
    ///     interface vor menu views
    /// </summary>
    public interface IMenuViewModel {

        /// <summary>
        ///     menu item text
        /// </summary>
        string Text { get; }

        /// <summary>
        ///     menu command
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        ///     menu items
        /// </summary>
        ObservableCollection<IMenuViewModel> MenuItems { get; }

    }
}