using System;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     common way to reference files
    /// </summary>
    /// <remarks>immutable</remarks>
    public class DesktopFileReference : IFileReference {

        private readonly string filePath;
        private readonly int hashcode;

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="path">path to fhe file</param>
        /// <exception cref="System.ArgumentException">Thrown if the path is empty</exception>
        public DesktopFileReference(string path) {

            if (string.IsNullOrWhiteSpace(path))
                ExceptionHelper.StringEmpty(nameof(path));

            filePath = path;
            hashcode = filePath.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        ///     get the path to the file
        /// </summary>
        public string Path
            => filePath;

        /// <summary>
        ///     name of the file (without path)
        /// </summary>
        public string FileName
            => System.IO.Path.GetFileName(filePath);

        /// <summary>
        ///     add a subpath
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        public IFileReference Append(IFileReference path)
            => new DesktopFileReference(System.IO.Path.Combine(filePath, path.Path));

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
            => hashcode;

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
