using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     definition of a resolved file
    /// </summary>
    public class ResolvedFile {

        /// <summary>
        ///     desciption of a resolved file
        /// </summary>
        /// <param name="currentDirectory">current directory</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <param name="target">target directory</param>
        /// <param name="isResolved"><c>true</c> if the file is resolved</param>
        public ResolvedFile(FileReference currentDirectory, FileReference pathToResolve, FileReference target, bool isResolved) {
            CurrentDirectory = currentDirectory ?? throw new ArgumentNullException(nameof(currentDirectory));
            PathToResolve = pathToResolve ?? throw new ArgumentNullException(nameof(pathToResolve));
            TargetPath = target ?? throw new ArgumentNullException(nameof(target));
            IsResolved = isResolved;
        }

        /// <summary>
        ///     current directory
        /// </summary>
        public FileReference CurrentDirectory { get; }

        /// <summary>
        ///     flag, <c>true</c> if the file exists
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public FileReference PathToResolve { get; }

        /// <summary>
        ///     target path
        /// </summary>
        public FileReference TargetPath { get; }
    }
}