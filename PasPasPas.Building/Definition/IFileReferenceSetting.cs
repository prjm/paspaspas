using PasPasPas.Infrastructure.Input;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     file references
    /// </summary>
    public interface IFileReferenceSetting {

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <returns></returns>
        IList<FileReference> GetReferencedFiles();

    }
}