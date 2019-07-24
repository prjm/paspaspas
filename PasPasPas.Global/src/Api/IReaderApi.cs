using PasPasPas.Globals.Files;

namespace PasPasPas.Globals.Api {

    /// <summary>
    ///     public interface for a file reader API
    /// </summary>
    public interface IReaderApi {

        /// <summary>
        ///     create an input object based on a file system file content
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns></returns>
        IReaderInput CreateInputForPath(string file);

        /// <summary>
        ///     create an input object based on a string file content
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        IReaderInput CreateInputForString(string path, string fileContent);

        /// <summary>
        ///     create a stacked file reader
        /// </summary>
        /// <param name="data">reader input</param>
        /// <returns>disposable reader</returns>
        IStackedFileReader CreateReader(IReaderInput data);
    }
}
