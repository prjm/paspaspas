using System.Collections.Generic;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     project definition
    /// </summary>
    public class Project {

        /// <summary>
        ///     project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     setting groups
        /// </summary>
        public IList<SettingGroup> Settings { get; }
            = new List<SettingGroup>();

        /// <summary>
        ///     project targets
        /// </summary>
        public IList<Target> Targets { get; }
            = new List<Target>();

    }
}
