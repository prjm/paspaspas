using PasPasPas.Building.Definition;
using System.Collections.Generic;

namespace PasPasPas.Building.Engine {

    /// <summary>
    ///     building engine
    /// </summary>
    public class ProjectBuilder {

        /// <summary>
        ///     build a project
        /// </summary>
        /// <param name="projectToBuild">project to build</param>
        /// <param name="settings">settings</param>
        public IList<object> BuildProject(Project projectToBuild, BuildSettings settings) {

            var work = new BuildWorkItem(projectToBuild, settings);
            return work.BuildProject();

        }

    }
}
