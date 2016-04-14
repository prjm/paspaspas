using P3Ide.ViewModel.MainWindow;

namespace P3Ide.ViewModel.StandardFiles {

    /// <summary>
    ///     supported file types
    /// </summary>
    public interface ISupportedFileType {

        /// <summary>
        ///     file extension
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        ///     file description
        /// </summary>
        string FileDescription { get; }

        /// <summary>
        ///     register an editor
        /// </summary>
        /// <param name="registry"></param>
        void RegisterEditor(IEditorRegistry registry);

    }
}