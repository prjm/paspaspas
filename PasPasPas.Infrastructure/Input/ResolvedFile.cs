﻿using System;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     definition of a resolved file
    /// </summary>
    public class ResolvedFile {

        /// <summary>
        ///     desciption of a resolved file
        /// </summary>
        /// <param name="currentDir">current directory</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <param name="target">target directory</param>
        /// <param name="isResolved"><c>true</c> if the file is resolved</param>
        public ResolvedFile(IFileReference currentDir, IFileReference pathToResolve, IFileReference target, bool isResolved) {

            if (currentDir == null)
                throw new ArgumentNullException(nameof(currentDir));

            if (pathToResolve == null)
                throw new ArgumentNullException(nameof(pathToResolve));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            CurrentDirectory = currentDir;
            PathToResolve = pathToResolve;
            TargetPath = target;
            IsResolved = isResolved;
        }

        /// <summary>
        ///     current directory
        /// </summary>
        public IFileReference CurrentDirectory { get; }

        /// <summary>
        ///     flag, <c>true</c> if the file exists
        /// </summary>
        public bool IsResolved { get; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public IFileReference PathToResolve { get; }

        /// <summary>
        ///     target path
        /// </summary>
        public IFileReference TargetPath { get; }
    }
}