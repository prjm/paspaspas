using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Building.Engine {

    /// <summary>
    ///     settings for a single build
    /// </summary>
    public class BuildSettings {

        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess FileSystemAccess { get; set; }

        /// <summary>
        ///     targets to build
        /// </summary>
        public IList<string> Targets { get; }
            = new List<string>();
    }
}