using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     file references
    /// </summary>
    public interface IFileReferenceSetting {

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <param name="env">used string pool</param>
        /// <returns></returns>
        IList<IFileReference> GetReferencedFiles(IEnvironment env);

    }
}