using System.Collections.Generic;
using PasPasPas.Building.Definition;
using PasPasPas.Building.Engine;

namespace PasPasPas.Building.Tasks {

    /// <summary>
    ///     base class for tasks
    /// </summary>
    public abstract class TaskBase : IBuildTask {

        /// <summary>
        ///     clear variables
        /// </summary>
        /// <param name="settings"></param>
        public virtual void ClearVariables(BuildSettings settings) {
            //..
        }


        /// <summary>
        ///     initialize variables
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="variables"></param>
        public virtual void InitializeVariables(BuildSettings settings, Dictionary<string, Setting> variables) {

        }

        /// <summary>
        ///     run task
        /// </summary>
        /// <param name="settings"></param>
        public abstract object Run(BuildSettings settings);
    }
}