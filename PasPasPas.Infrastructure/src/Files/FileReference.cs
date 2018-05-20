﻿using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     common way to reference files
    /// </summary>
    /// <remarks>immutable</remarks>
    public class FileReference : IFileReference {

        private readonly int hashcode;

        /// <summary>
        ///     create a new file reference
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <exception cref="System.ArgumentException">Thrown if the path is empty</exception>
        public FileReference(string filePath) {
            Path = filePath;

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "Invalid path.");

            hashcode = filePath.ToUpperInvariant().GetHashCode();
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

            if (other is null)
                return false;

            return string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
