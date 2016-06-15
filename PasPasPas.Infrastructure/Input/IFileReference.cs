namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     reference to a file
    /// </summary>
    public interface IFileReference {

        /// <summary>
        ///     get file path
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     add a path segment to this path
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        IFileReference Append(IFileReference path);
    }
}
