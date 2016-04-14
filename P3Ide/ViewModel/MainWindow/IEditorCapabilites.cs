using P3Ide.ViewModel.Projects;
using P3Ide.ViewModel.StandardFiles;
using System.Collections.Generic;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     editor capabilites
    /// </summary>
    public interface IEditorCapabilites {

        /// <summary>
        ///     supported project types
        /// </summary>
        IList<ISupportedProjectType> SupportedProjectTypes { get; }

        /// <summary>
        ///     supported file types
        /// </summary>
        IList<ISupportedFileType> SupportedFileTypes { get; }

        /// <summary>
        ///     editor registry
        /// </summary>
        IEditorRegistry Registry { get; }

    }
}