using PasPasPas.Building.Definition;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Building.Engine {

    /// <summary>
    ///     a build work item
    /// </summary>
    public class BuildWorkItem {

        /// <summary>
        ///     project to build
        /// </summary>
        private readonly Project projectToBuild;

        /// <summary>
        ///     build settings
        /// </summary>
        private readonly BuildSettings settings;

        /// <summary>
        ///     create a new work item
        /// </summary>
        /// <param name="projectToBuild">project definition</param>
        /// <param name="settings">settings</param>
        public BuildWorkItem(Project projectToBuild, BuildSettings settings) {
            this.projectToBuild = projectToBuild;
            this.settings = settings;
        }

        /// <summary>
        ///     build project
        /// </summary>
        public IList<object> BuildProject() {
            var result = new List<object>();
            var targetsInOrder = new List<string>();

            targetsInOrder.AddRange(settings.Targets);

            foreach (var target in targetsInOrder) {
                Target targetToRun = projectToBuild.Targets.Single(t => string.Equals(t.Name, target, System.StringComparison.OrdinalIgnoreCase));

                foreach (IBuildTask task in targetToRun.Tasks) {
                    var taskResult = RunTask(task);

                    if (taskResult != null)
                        result.Add(taskResult);
                }
            }

            return result;

        }

        private object RunTask(IBuildTask task) {
            IEnumerable<Setting> variables = projectToBuild.Settings.SelectMany(t => t.Items);
            var indexedVariables = new Dictionary<string, Setting>();
            foreach (Setting variable in variables)
                if (!indexedVariables.ContainsKey(variable.Name))
                    indexedVariables.Add(variable.Name, variable);

            task.ClearVariables(settings);
            task.InitializeVariables(settings, indexedVariables);
            return task.Run(settings);
        }
    }
}

