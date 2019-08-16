using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;

namespace PasPasPas.Globals.Api {

    /// <summary>
    ///     input resolver
    /// </summary>
    /// <param name="path"></param>
    /// <param name="api">API</param>
    /// <returns></returns>
    public delegate IReaderInput Resolver(FileReference path, IReaderApi api);

    /// <summary>
    ///     file existence checker
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public delegate bool Checker(FileReference path);

    /// <summary>
    ///     public interface for a file reader API
    /// </summary>
    public interface IReaderApi {

        /// <summary>
        ///     system environment
        /// </summary>
        IEnvironment SystemEnvironment { get; }

        /// <summary>
        ///     create a fie reference
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        FileReference CreateFileRef(string path);

        /// <summary>
        ///     create an input object based on a file system file content
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns></returns>
        IReaderInput CreateInputForPath(FileReference file);

        /// <summary>
        ///     create an input object based on a string file content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        IReaderInput CreateInputForString(FileReference path, string fileContent);

        /// <summary>
        ///     create a stacked file reader
        /// </summary>
        /// <param name="input">reader input</param>
        /// <param name="file">file reference</param>
        /// <returns>disposable reader</returns>
        IStackedFileReader CreateReader(IInputResolver input, FileReference file);

    }
}
