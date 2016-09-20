using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;

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
        public IList<FileReference> GetRerencedFiles() {
            var result = new List<FileReference>();
            result.Add(new FileReference(Path));
            return result;
        }
    }
}
