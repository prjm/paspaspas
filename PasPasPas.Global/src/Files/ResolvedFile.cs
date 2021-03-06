﻿#nullable disable
using System;

namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     definition of a resolved file
    /// </summary>
    public class ResolvedFile {

        /// <summary>
        ///     description of a resolved file
        /// </summary>
        /// <param name="currentDirectory">current directory</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <param name="target">target directory</param>
        /// <param name="isResolved"><c>true</c> if the file is resolved</param>
        public ResolvedFile(IFileReference currentDirectory, IFileReference pathToResolve, IFileReference target, bool isResolved) {
            CurrentDirectory = currentDirectory ?? throw new ArgumentNullException(nameof(currentDirectory));
            PathToResolve = pathToResolve ?? throw new ArgumentNullException(nameof(pathToResolve));
            TargetPath = target ?? throw new ArgumentNullException(nameof(target));
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