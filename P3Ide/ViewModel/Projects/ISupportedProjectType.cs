namespace P3Ide.ViewModel.Projects {

    /// <summary>
    ///     supported project types
    /// </summary>
    public interface ISupportedProjectType {

        /// <summary>
        ///     project file extension
        /// </summary>
        string ProjectExtension { get; }

        /// <summary>
        ///     project file description
        /// </summary>
        string ProjectDescription { get; }

        /// <summary>
        ///     open project
        /// </summary>
        /// <param name="path">path to project file</param>
        /// <returns>project to open</returns>
        IProject OpenProject(string path);

    }
}