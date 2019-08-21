namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     file reference
    /// </summary>
    public interface IFileReference {

        /// <summary>
        ///     referenced path
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     file name
        /// </summary>
        string FileName { get; }

        /// <summary>
        ///     append a child path
        /// </summary>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        IFileReference Append(IFileReference pathToResolve);

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IFileReference CreateNewFileReference(string path);
    }
}
