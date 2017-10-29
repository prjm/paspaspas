using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     file references
    /// </summary>
    public interface IFileReferenceSetting {

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <param name="fileAccess">file access</param>
        /// <param name="pool">used string pool</param>
        /// <returns></returns>
        IList<IFileReference> GetReferencedFiles(StringPool pool, IFileAccess fileAccess);

    }
}