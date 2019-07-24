using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     file references
    /// </summary>
    public interface IFileReferenceSetting {

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <param name="pool">used string pool</param>
        /// <returns></returns>
        IList<FileReference> GetReferencedFiles(StringPool pool);

    }
}