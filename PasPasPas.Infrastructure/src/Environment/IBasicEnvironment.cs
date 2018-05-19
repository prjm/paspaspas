using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.ObjectPooling;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     interface for basic environment services
    /// </summary>
    public interface IBasicEnvironment {

        /// <summary>
        ///     logging
        /// </summary>
        ILogManager Log { get; }

        /// <summary>
        ///     file access
        /// </summary>
        IFileAccess Files { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        StringBuilderPool StringBuilderPool { get; }

        /// <summary>
        ///     string pool
        /// </summary>
        StringPool StringPool { get; }

        /// <summary>
        ///     enumerated entries
        /// </summary>
        IEnumerable<object> Entries { get; }

    }
}
