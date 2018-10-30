using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for file reading
    /// </summary>
    public class ReaderApi {

        /// <summary>
        ///     file access
        /// </summary>
        private readonly IBasicEnvironment systemEnvironment;

        /// <summary>
        ///     create a new reader for a virtual file
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="content">file content</param>
        /// <returns>file reader</returns>
        public StackedFileReader CreateReaderForString(string path, string content) {
            var localPath = new FileReference(path);
            var reader = new StackedFileReader();
            reader.AddStringToRead(localPath, content);
            return reader;
        }

        /// <summary>
        ///     create a new file reader
        /// </summary>
        /// <param name="environment">environment</param>
        public ReaderApi(IBasicEnvironment environment)
            => systemEnvironment = environment;

        /// <summary>
        ///     get a reader for a given path
        /// </summary>
        /// <param name="path">path to resolve</param>
        /// <returns>file reader</returns>
        public StackedFileReader CreateReaderForPath(string path) {
            var localPath = new FileReference(path);
            var reader = new StackedFileReader();
            reader.AddFileToRead(localPath);
            return reader;
        }

        /// <summary>
        ///     switch to another path
        /// </summary>
        /// <param name="reader">reader to switch</param>
        /// <param name="path">path to ope</param>
        public static void SwitchToPath(StackedFileReader reader, string path) {
            var localPath = new FileReference(path);
            reader.AddFileToRead(localPath);
        }
    }
}
