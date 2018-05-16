using System.Collections.Generic;
using PasPasPas.Building.Engine;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     interface for tasks
    /// </summary>
    public interface IBuildTask {

        /// <summary>
        ///     run the task
        /// </summary>
        /// <param name="settings">build settings</param>
        object Run(BuildSettings settings);

        /// <summary>
        ///     clear variables
        /// </summary>
        /// <param name="settings"></param>
        void ClearVariables(BuildSettings settings);

        /// <summary>
        ///     initialize task variables
        /// </summary>
        /// <param name="variables">variables</param>
        /// <param name="settings">settings for this build</param>
        void InitializeVariables(BuildSettings settings, Dictionary<string, Setting> variables);
    }
}