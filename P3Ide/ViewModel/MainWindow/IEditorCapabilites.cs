using P3Ide.ViewModel.Projects;
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

    }
}