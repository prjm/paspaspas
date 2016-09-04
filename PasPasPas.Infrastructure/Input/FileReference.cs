using System;

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

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"Invalid path {path}", nameof(path));

            this.path = path;
        }

        /// <summary>
        ///     get the path to the file
        /// </summary>
        public string Path
            => path;

        /// <summary>
        ///     file name of the file
        /// </summary>
        public string FileName
            => System.IO.Path.GetFileName(path);

        /// <summary>
        ///     add a subpath
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        public IFileReference Append(IFileReference path)
            => new FileReference(System.IO.Path.Combine(this.path, path.Path));

        /// <summary>
        ///     string representation of this file reference
        /// </summary>
        /// <returns>path</returns>
        public override string ToString()
            => path;

        /// <summary>
        ///     get the hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => path.ToLowerInvariant().GetHashCode();

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var other = obj as IFileReference;

            if (ReferenceEquals(obj, null))
                return false;

            return string.Equals(path, other.Path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
