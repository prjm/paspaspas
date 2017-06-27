using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     service to access files
    /// </summary>
    public interface IFileAccess {


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
        /// <returns>file reference</returns>
        IFileReference ReferenceToFile(string path);
    }
}
