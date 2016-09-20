using System.Collections.Generic;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     build target
    /// </summary>
    public class Target {

        /// <summary>
        ///     target name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     list of tasks
        /// </summary>
        public IList<IBuildTask> Tasks { get; }
        = new List<IBuildTask>();

    }
}