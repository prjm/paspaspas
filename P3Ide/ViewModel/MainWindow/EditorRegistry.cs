using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     editor registry
    /// </summary>
    public class EditorRegistry : IEditorRegistry {

        private IDictionary<string, Func<EditorViewModel>> registeredEditors
            = new Dictionary<string, Func<EditorViewModel>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     register a file type
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="viewModel"></param>
        public void RegisterFileType(string extension, Func<EditorViewModel> viewModel) {
            registeredEditors.Add(extension, viewModel);
        }

        /// <summary>
        ///     create an editor for a file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public EditorViewModel TryToCreateEditorForFile(string path) {
            string extension = Path.GetExtension(path);
            Func<EditorViewModel> editorCreator;
            if (registeredEditors.TryGetValue(extension, out editorCreator)) {
                EditorViewModel model = editorCreator();
                return model;
            }
            return null;
        }
    }
}
