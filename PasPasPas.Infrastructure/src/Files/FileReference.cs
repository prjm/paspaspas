#nullable disable
using System;
using System.IO;
using PasPasPas.Globals.Files;

#if DESKTOP
using PasPasPas.Desktop.BackwardCompatibility;
#endif

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     common way to reference files
    /// </summary>
    /// <remarks>immutable</remarks>
    internal class FileReference : IFileReference, IEquatable<IFileReference> {

        private readonly int hashcode;

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <exception cref="System.ArgumentException">Thrown if the path is empty</exception>
        internal FileReference(string filePath) {
            Path = filePath;

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "Invalid path.");

            hashcode = filePath.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     get the path to the file
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     name of the file (without path)
        /// </summary>
        public string FileName
            => System.IO.Path.GetFileName(Path);

        /// <summary>
        ///     add a child path
        /// </summary>
        /// <param name="path">path to add</param>
        /// <returns>combined path</returns>
        public IFileReference Append(IFileReference path)
            => new FileReference(System.IO.Path.Combine(Path, path.Path));

        /// <summary>
        ///     string representation of this file reference
        /// </summary>
        /// <returns>path</returns>
        public override string ToString()
            => Path;

        /// <summary>
        ///     get the hash code
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
            if (!(obj is IFileReference other))
                return false;

            return string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        public IFileReference CreateNewFileReference(string path)
            => new FileReference(path);

        public bool Equals(IFileReference other)
            => string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);

        public IFileReference GetCurrentDirectory()
            => CreateNewFileReference(Directory.GetCurrentDirectory());

        public string GetDirectory()
            => System.IO.Path.GetDirectoryName(Path);
    }
}
