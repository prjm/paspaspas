#nullable disable
using System.IO;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     stacked file reader input for files
    /// </summary>
    internal class FileReaderInput : IReaderInput {

        /// <summary>
        ///     create a new file reader input
        /// </summary>
        /// <param name="path"></param>
        public FileReaderInput(IFileReference path)
            => Path = path;

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference Path { get; }

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
            var stream = new FileStream(Path.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new Utf8StreamBufferSource(stream, BufferSize, BufferSize);
        }
    }
}
