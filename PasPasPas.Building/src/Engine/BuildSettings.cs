#nullable disable
using System.Collections.Generic;

namespace PasPasPas.Building.Engine {

    /// <summary>
    ///     settings for a single build
    /// </summary>
    public class BuildSettings {

        /// <summary>
        ///     targets to build
        /// </summary>
        public IList<string> Targets { get; }
            = new List<string>();
    }
}