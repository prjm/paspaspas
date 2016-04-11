using System.Collections.ObjectModel;

namespace P3Ide.ViewModel {

    /// <summary>
    ///     view model for the dock manager
    /// </summary>
    public class DockManagerViewModel {

        /// <summary>
        ///     documents
        /// </summary>
        public ObservableCollection<DockWindowViewModel> Documents { get; }

        /// <summary>
        ///     docked windows
        /// </summary>
        public ObservableCollection<object> Anchorables { get; }

        /// <summary>
        ///     view model for the dock manager
        /// </summary>
        public DockManagerViewModel() {
            Documents = new ObservableCollection<DockWindowViewModel>();
            Anchorables = new ObservableCollection<object>();


        }

    }
}
