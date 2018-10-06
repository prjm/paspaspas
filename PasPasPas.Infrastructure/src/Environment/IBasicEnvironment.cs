using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.ObjectPooling;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     interface for basic environment services
    /// </summary>
    public interface IBasicEnvironment {

        /// <summary>
        ///     logging / error reporting
        /// </summary>
        ILogManager Log { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        StringBuilderPool StringBuilderPool { get; }

        /// <summary>
        ///     string pool
        /// </summary>
        StringPool StringPool { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        ListPools ListPools { get; }

        /// <summary>
        ///     enumerated entries (used for statistics)
        /// </summary>
        IEnumerable<object> Entries { get; }

    }
}
