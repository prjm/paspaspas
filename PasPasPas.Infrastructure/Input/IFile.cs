using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     common interface for files
    /// </summary>
    public interface IFile {


        /// <summary>
        ///     file path
        /// </summary>
        IFileReference FilePath { get; }

    }
}
