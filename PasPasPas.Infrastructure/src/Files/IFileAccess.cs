using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     service to access files
    /// </summary>
    public interface IFileAccess {

        /// <summary>
        ///    define a mockup / virtual file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        void AddMockupFile(IFileReference path, IBufferReadable content);

        /// <summary>
        ///     open a file for reading
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>input file</returns>
        IBufferReadable OpenFileForReading(IFileReference path);

        /// <summary>
        ///     test if a files exists
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns><c>true</c> if the file exists</returns>
        bool FileExists(IFileReference filePath);

        /// <summary>
        ///     create a reference to a file
        /// </summary>
        /// <param name="path">file to reference</param>
        /// <param name="pool">string pool to use</param>
        /// <returns>file reference</returns>
        IFileReference ReferenceToFile(StringPool pool, string path);
    }
}
