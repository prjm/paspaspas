using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     delegeate to create editors
    /// </summary>
    /// <param name="model">model for the main window</param>
    /// <param name="workspace">workspace model</param>
    /// <returns></returns>
    public delegate EditorViewModel EditorCreator(IEditorCapabilites model, IEditorWorkspace workspace);


    /// <summary>
    ///     editor registry
    /// </summary>
    public interface IEditorRegistry {

        /// <summary>
        ///     Register a file type
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <param name="viewModel">registered view model</param>
        void RegisterFileType(string extension, EditorCreator viewModel);

        /// <summary>
        ///     create a editor for a given file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        EditorViewModel TryToCreateEditorForFile(string path);

    }
}
