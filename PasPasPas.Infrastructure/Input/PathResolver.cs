﻿using System;
using System.Collections.Generic;
using System.IO;

namespace PasPasPas.Infrastructure.Input {

    internal class ResolvedPathKey {

        /// <summary>
        ///     base path
        /// </summary>
        public IFileReference BasePath { get; set; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public IFileReference PathToResolve { get; set; }

        /// <summary>
        ///     compare to keys for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var otherKey = obj as ResolvedPathKey;

            if (ReferenceEquals(otherKey, null)) return false;

            var basePathEquals = BasePath.Equals(otherKey.BasePath);
            var resolvePathEquals = PathToResolve.Equals(otherKey.PathToResolve);

            return basePathEquals && resolvePathEquals;
        }

        /// <summary>
        ///     compute a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int result = 17;
            result = result * 31 + BasePath.GetHashCode();
            result = result * 31 + PathToResolve.GetHashCode();
            return result;
        }

    }

    /// <summary>
    ///     resolves paths for common scenarios
    /// </summary>
    public abstract class PathResolver {

        private IDictionary<ResolvedPathKey, ResolvedFile> resolvedPaths
            = new Dictionary<ResolvedPathKey, ResolvedFile>();

        /// <summary>
        ///     access to files
        /// </summary>
        public IFileAccess Files { get; }


        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="fileAccess">file access</param>
        protected PathResolver(IFileAccess fileAccess) {
            Files = fileAccess;
        }

        /// <summary>
        ///     resolves a path and caches the result
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns>resolved path</returns>
        public ResolvedFile ResolvePath(IFileReference basePath, IFileReference pathToResolve) {

            var key = new ResolvedPathKey() {
                BasePath = basePath,
                PathToResolve = pathToResolve
            };

            ResolvedFile result;

            if (!resolvedPaths.TryGetValue(key, out result)) {
                result = DoResolvePath(basePath, pathToResolve);
                resolvedPaths.Add(key, result);
            }

            return result;
        }

        /// <summary>
        ///     check if a given path exists in a directory
        /// </summary>
        /// <param name="currentDirectory">current directoy</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns></returns>
        protected ResolvedFile ResolveInDirectory(IFileReference currentDirectory, IFileReference pathToResolve) {
            var fileAccess = Files;
            IFileReference targetPath;
            IFileReference combinedPath = currentDirectory.Append(pathToResolve);

            if (fileAccess.FileExists(combinedPath))
                targetPath = combinedPath;
            else
                targetPath = null;

            return new ResolvedFile() {
                IsResolved = targetPath != null,
                CurrentDirectory = currentDirectory,
                PathToResolve = pathToResolve,
                TargetPath = targetPath
            };
        }

        /// <summary>
        ///     resolve a path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected abstract ResolvedFile DoResolvePath(IFileReference basePath, IFileReference pathToResolve);
    }
}
