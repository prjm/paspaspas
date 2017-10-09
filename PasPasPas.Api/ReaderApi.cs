using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for file reading api
    /// </summary>
    public class ReaderApi {

        /// <summary>
        ///     file access
        /// </summary>
        private IBasicEnvironment staticEnvironment;

        /// <summary>
        ///     file buffer
        /// </summary>
        private FileBuffer fileBuffer;

        /// <summary>
        ///     create a new reader for a virtual file
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="content">file content</param>
        /// <returns>file reader</returns>
        public StackedFileReader CreateReaderForString(string path, string content) {
            var fileAccess = staticEnvironment.Files;
            var localPath = fileAccess.ReferenceToFile(path);
            var reader = new StackedFileReader(fileBuffer);
            fileBuffer.Add(localPath, new StringBufferReadable(content));
            reader.AddFileToRead(localPath);
            return reader;
        }

        /// <summary>
        ///     create a new file reader api
        /// </summary>
        /// <param name="environment">environment</param>
        public ReaderApi(IBasicEnvironment environment) {
            staticEnvironment = environment;
            fileBuffer = new FileBuffer();
        }

        /// <summary>
        ///     get a reader for a given path
        /// </summary>
        /// <param name="path">path to resolve</param>
        /// <returns>file reader</returns>
        public StackedFileReader CreateReaderForPath(string path) {
            var fileAccess = staticEnvironment.Files;
            var localPath = fileAccess.ReferenceToFile(path);
            var reader = new StackedFileReader(fileBuffer);
            fileBuffer.Add(localPath, fileAccess.OpenFileForReading(localPath));
            reader.AddFileToRead(localPath);
            return reader;
        }

        /// <summary>
        ///     switch to another path
        /// </summary>
        /// <param name="reader">reader to switch</param>
        /// <param name="path">path to ope</param>
        public void SwitchToPath(StackedFileReader reader, string path) {
            var fileAccess = staticEnvironment.Files;
            var localPath = fileAccess.ReferenceToFile(path);
            fileBuffer.Add(localPath, fileAccess.OpenFileForReading(localPath));
            reader.AddFileToRead(localPath);
        }
    }
}
