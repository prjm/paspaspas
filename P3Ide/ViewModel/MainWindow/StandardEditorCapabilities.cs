using System;
using System.Collections.Generic;
using P3Ide.ViewModel.Projects;
using P3Ide.ViewModel.StandardFiles;

namespace P3Ide.ViewModel.MainWindow {

    /// <summary>
    ///     standard editor capabilities
    /// </summary>
    public class StandardEditorCapabilities : IEditorCapabilites {

        /// <summary>
        ///     create new editor capabilities
        /// </summary>
        /// <param name="supportedProjectTypes">supported project types</param>
        /// <param name="supportedFileTypes">supported file types</param>
        /// <param name="workspace">workspace</param>
        public StandardEditorCapabilities(IEnumerable<ISupportedProjectType> supportedProjectTypes, IEnumerable<ISupportedFileType> supportedFileTypes, IEditorWorkspace workspace) {
            SupportedProjectTypes = new List<ISupportedProjectType>(supportedProjectTypes);
            SupportedFileTypes = new List<ISupportedFileType>(supportedFileTypes);
            Registry = new EditorRegistry(this, workspace);

            foreach (var fileType in SupportedFileTypes) {
                fileType.RegisterEditor(Registry);
            }
        }

        /// <summary>
        ///     get supported project types
        /// </summary>
        public IList<ISupportedProjectType> SupportedProjectTypes { get; }

        /// <summary>
        ///     supported file type
        /// </summary>
        public IList<ISupportedFileType> SupportedFileTypes { get; }

        /// <summary>
        ///     editor registry
        /// </summary>
        public IEditorRegistry Registry { get; }
    }
}
