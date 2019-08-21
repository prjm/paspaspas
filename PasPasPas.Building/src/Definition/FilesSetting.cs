using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     reference to one or more files
    /// </summary>
    public class FilesSetting : Setting, IFileReferenceSetting {

        /// <summary>
        ///     input path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     get a list of referenced files
        /// </summary>
        /// <returns></returns>
        public IList<IFileReference> GetReferencedFiles(IEnvironment env)
            => new List<IFileReference> {
                env.CreateFileReference(Path)
            };
    }
}
