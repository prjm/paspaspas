using System.IO;
using System.Text;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     interface for file reader creation
    /// </summary>
    public interface IFileReaderFactory {


        /// <summary>
        ///     create file reader
        /// </summary>               
        StreamReader CreateReader(string fileName, Encoding encoding);

    }
}
