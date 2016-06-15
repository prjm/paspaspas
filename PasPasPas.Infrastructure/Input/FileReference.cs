using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     a simple file reference
    /// </summary>
    public class FileReference : IFileReference {

        private readonly string path;

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="path">file path</param>
        public FileReference(string path) {
            this.path = path;
        }

        /// <summary>
        ///     get the path to the file
        /// </summary>
        public string Path
            => path;

        /// <summary>
        ///     add a subpath
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        public IFileReference Append(IFileReference path)
            => new FileReference(System.IO.Path.Combine(this.path ?? string.Empty, path.Path));
    }
}
