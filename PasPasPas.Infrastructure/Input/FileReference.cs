using System;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     a simple file reference
    /// </summary>
    public class FileReference : IFileReference {

        private readonly string filePath;

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="path">file path</param>
        public FileReference(string path) {

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(StringUtils.Invariant($"Invalid path {path}"), nameof(path));

            filePath = path;
        }

        /// <summary>
        ///     get the path to the file
        /// </summary>
        public string Path
            => filePath;

        /// <summary>
        ///     file name of the file
        /// </summary>
        public string FileName
            => System.IO.Path.GetFileName(filePath);

        /// <summary>
        ///     add a subpath
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        public IFileReference Append(IFileReference path)
            => new FileReference(System.IO.Path.Combine(filePath, path.Path));

        /// <summary>
        ///     string representation of this file reference
        /// </summary>
        /// <returns>path</returns>
        public override string ToString()
            => filePath;

        /// <summary>
        ///     get the hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => filePath.ToUpperInvariant().GetHashCode();

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var other = obj as IFileReference;

            if (ReferenceEquals(other, null))
                return false;

            return string.Equals(filePath, other.Path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
