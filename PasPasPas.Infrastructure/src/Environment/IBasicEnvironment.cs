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
        ///     enumerated entries
        /// </summary>
        IEnumerable<object> Entries { get; }

        /// <summary>
        ///     add a mockup file
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="content"></param>
        void AddMockupFile(FileReference inputFile, string content);

    }
}
