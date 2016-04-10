using System.Collections.Generic;
using P3Ide.ViewModel.Projects;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     standard editor capabilities
    /// </summary>
    public class StandardEditorCapabilities : IEditorCapabilites {

        /// <summary>
        ///     create new editor capabilities
        /// </summary>
        /// <param name="supportedProjectTypes">supported project types</param>
        public StandardEditorCapabilities(IEnumerable<ISupportedProjectType> supportedProjectTypes) {
            SupportedProjectTypes = new List<ISupportedProjectType>(supportedProjectTypes);
        }

        /// <summary>
        ///     get supported project types
        /// </summary>
        public IList<ISupportedProjectType> SupportedProjectTypes { get; }

    }
}
