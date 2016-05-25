using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3Ide.ViewModel.MainWindow;

namespace P3Ide.ViewModel.StandardFiles {

    /// <summary>
    ///     text file type
    /// </summary>
    public class TextFileType : ISupportedFileType {

        /// <summary>
        ///     file description
        /// </summary>
        public string FileDescription
            => "Text file";

        /// <summary>
        ///     file extension
        /// </summary>
        public string FileExtension
            => ".txt";

        /// <summary>
        ///     register an editor
        /// </summary>
        /// <param name="registry"></param>
        public void RegisterEditor(IEditorRegistry registry) {
            EditorCreator editor = (_1, _2) => {
                return new TextEditorViewModel();
            };
            registry.RegisterFileType(FileExtension, editor);
        }
    }
}
