using System.Collections.ObjectModel;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     editor worskapce 
    /// </summary>
    public interface IEditorWorkspace {


        /// <summary>
        ///     documents to edit
        /// </summary>
        ObservableCollection<EditorViewModel> Documents { get; }

    }
}