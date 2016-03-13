using System.IO;
using System.Text;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     file reader factory
    /// </summary>
    public class FileReaderFactory : IFileReaderFactory {

        /// <summary>
        ///     create a new file reader
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public StreamReader CreateReader(string fileName, Encoding encoding)
            => new StreamReader(fileName, encoding);

        /// <summary>
        ///     register this factory
        /// </summary>
        public static void Register() {
            if (FileReaderFactories.Default != null) {
                FileReaderFactories.Default = new FileReaderFactory();
            }
        }
    }
}
