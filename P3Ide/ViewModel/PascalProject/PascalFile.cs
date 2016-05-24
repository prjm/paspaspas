using P3Ide.ViewModel.StandardFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3Ide.ViewModel.MainWindow;

namespace P3Ide.ViewModel.PascalProject {

    /// <summary>
    ///     supported file type: pascal file
    /// </summary>
    public class PascalFileType : ISupportedFileType {

        /// <summary>
        ///     file description
        /// </summary>
        public string FileDescription
            => "Pascal File";

        /// <summary>
        ///     file extensions
        /// </summary>
        public string FileExtension
            => ".pas";

        /// <summary>
        ///     editor
        /// </summary>
        /// <param name="registry"></param>
        public void RegisterEditor(IEditorRegistry registry) {
            Func<EditorViewModel> editor = () => {
                return new PascalEditorViewModel();
            };
            registry.RegisterFileType(FileExtension, editor);
        }
    }
}
