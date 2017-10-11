using System.Collections.Generic;
using System.Text;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     interface for basic environment services
    /// </summary>
    public interface IBasicEnvironment {

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager Log { get; }

        /// <summary>
        ///     file access
        /// </summary>
        IFileAccess Files { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        ObjectPool<StringBuilder> StringBuilderPool { get; }

        /// <summary>
        ///     string pool for single chars
        /// </summary>
        CharsAsString CharStringPool { get; }

        /// <summary>
        ///     enumerated entries
        /// </summary>
        IEnumerable<object> Entries { get; }
    }
}
