using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for file reading
    /// </summary>
    internal class ReaderApi : IReaderApi {

        /// <summary>
        ///     create a new file reader API
        /// </summary>
        /// <param name="environment">environment</param>
        public ReaderApi(IEnvironment environment)
            => SystemEnvironment = environment;

        /// <summary>
        ///    system environment
        /// </summary>
        public IEnvironment SystemEnvironment { get; }

        /// <summary>
        ///     create a new reader for input resolver
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="file"></param>
        /// <returns>file reader</returns>
        public IStackedFileReader CreateReader(IInputResolver input, IFileReference file) {
            var reader = new StackedFileReader(this, input);
            reader.AddInputToRead(file);
            return reader;
        }

        /// <summary>
        ///     create an input for a string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public IReaderInput CreateInputForString(IFileReference path, string input)
            => new StringReaderInput(path, input);

        /// <summary>
        ///     create an input for a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IReaderInput CreateInputForPath(IFileReference path)
            => new FileReaderInput(path);

    }
}
