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

        private IDictionary<string, EditorCreator> registeredEditors
            = new Dictionary<string, EditorCreator>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     create a new registry
        /// </summary>
        /// <param name="capabilities"></param>
        /// <param name="workspace">workspace</param>
        public EditorRegistry(IEditorCapabilites capabilities, IEditorWorkspace workspace) {
            Capabilities = capabilities;
            Workspace = workspace;
        }

        /// <summary>
        ///     capabilties
        /// </summary>
        public IEditorCapabilites Capabilities { get; }

        /// <summary>
        ///     editor workspace
        /// </summary>
        public IEditorWorkspace Workspace { get; }

        /// <summary>
        ///     register a file type
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="viewModel"></param>
        public void RegisterFileType(string extension, EditorCreator viewModel) {
            registeredEditors.Add(extension, viewModel);
        }

        /// <summary>
        ///     create an editor for a file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public EditorViewModel TryToCreateEditorForFile(string path) {
            string extension = Path.GetExtension(path);
            EditorCreator editorCreator;
            if (registeredEditors.TryGetValue(extension, out editorCreator)) {
                EditorViewModel model = editorCreator(Capabilities, Workspace);
                return model;
            }
            return null;
        }
    }
}
