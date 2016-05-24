using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     editor workspace
    /// </summary>
    public class EditorWorkspace : IEditorWorkspace {

        /// <summary>
        ///     documents
        /// </summary>
        public ObservableCollection<EditorViewModel> Documents { get; } =
            new ObservableCollection<EditorViewModel>();

        /// <summary>
        ///     tool windows
        /// </summary>
        public ObservableCollection<ToolWindowViewModel> ToolWindows { get; } =
            new ObservableCollection<ToolWindowViewModel>();
    }
}
