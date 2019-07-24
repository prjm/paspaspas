using System.IO;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     stacked file reader input for files
    /// </summary>
    public class FileReaderInput : IReaderInput {

        /// <summary>
        ///     create a new file reader input
        /// </summary>
        /// <param name="path"></param>
        public FileReaderInput(string path)
            => Path = path;

        /// <summary>
        ///     file path
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     fixed buffer size
        /// </summary>
        public int BufferSize
            => 1024;

        /// <summary>
        ///     create a new buffer source
        /// </summary>
        /// <returns></returns>
        public IBufferSource CreateBufferSource() {
            var stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new Utf8StreamBufferSource(stream, BufferSize, BufferSize);
        }
    }
}
