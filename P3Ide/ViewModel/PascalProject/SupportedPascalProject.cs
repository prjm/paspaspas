using P3Ide.ViewModel.Projects;

namespace P3Ide.ViewModel.PascalProject {

    /// <summary>
    ///     support for pascal projects
    /// </summary>
    public class SupportedPascalProject : ISupportedProjectType {

        /// <summary>
        ///     description
        /// </summary>
        public string ProjectDescription
            => "Pascal Project (dpr)";

        /// <summary>
        ///     project extensions
        /// </summary>
        public string ProjectExtension
            => ".dpr";

        /// <summary>
        ///     open the project
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IProject OpenProject(string path) => null;
    }
}
