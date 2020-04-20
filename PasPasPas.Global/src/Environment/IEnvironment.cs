using System.Text;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     interface for a common environment
    /// </summary>
    public interface IEnvironment {

        /// <summary>
        ///     logging / error reporting
        /// </summary>
        ILogManager Log { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        IListPools ListPools { get; }

        /// <summary>
        ///     string pool
        /// </summary>
        IStringPool StringPool { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        IObjectPool<StringBuilder> StringBuilderPool { get; }

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IFileReference CreateFileReference(string path);
    }
}
