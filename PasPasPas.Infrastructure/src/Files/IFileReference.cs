using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     reference to a file
    /// </summary>
    public interface IFileReference {

        /// <summary>
        ///     get only the file name
        /// </summary>
        string FileName { get; }

        /// <summary>
        ///     get file path
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     add a path segment to this path
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        IFileReference Append(StringPool pool, IFileReference path);
    }
}
