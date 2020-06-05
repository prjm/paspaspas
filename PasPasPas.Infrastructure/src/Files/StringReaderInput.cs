#nullable disable
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     input based on a string
    /// </summary>
    internal class StringReaderInput : IReaderInput {

        /// <summary>
        ///     create a new input object for strings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="input"></param>
        public StringReaderInput(IFileReference path, string input) {
            Path = path;
            Content = input;
        }

        /// <summary>
        ///     file content
        /// </summary>
        public string Content { get; }

        /// <summary>
        ///     file name
        /// </summary>
        public IFileReference Path { get; }

        /// <summary>
        ///     fixed buffer size
        /// </summary>
        public int BufferSize
            => 1024;

        /// <summary>
        ///     create a buffer source
        /// </summary>
        /// <returns></returns>
        public IBufferSource CreateBufferSource()
            => new StringBufferSource(Content);
    }
}
