using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {


    /// <summary>
    ///     editor registry
    /// </summary>
    public interface IEditorRegistry {

        /// <summary>
        ///     Register a file type
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <param name="viewModel">registered view model</param>
        void RegisterFileType(string extension, Func<EditorViewModel> viewModel);

        /// <summary>
        ///     create a editor for a given file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        EditorViewModel TryToCreateEditorForFile(string path);

    }
}
