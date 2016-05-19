using System;
using System.Collections.Generic;
using System.IO;

namespace PasPasPas.Infrastructure.Input {

    internal class ResolvedPathKey {

        /// <summary>
        ///     base path
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public string PathToResolve { get; set; }

        /// <summary>
        ///     compare to keys for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var otherKey = obj as ResolvedPathKey;

            if (ReferenceEquals(otherKey, null)) return false;

            return string.Equals(BasePath, otherKey.BasePath, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(PathToResolve, otherKey.PathToResolve, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     compute a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int result = 17;
            result = result * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(BasePath);
            result = result * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(PathToResolve);
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
        public ResolvedFile ResolvePath(string basePath, string pathToResolve) {

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
        protected ResolvedFile ResolveInDirectory(string currentDirectory, string pathToResolve) {
            var fileAccess = Files;
            string combinedPath;
            string targetPath;

            if (!string.IsNullOrEmpty(currentDirectory))
                combinedPath = Path.Combine(currentDirectory, pathToResolve);
            else
                combinedPath = pathToResolve;

            if (fileAccess.FileExists(combinedPath))
                targetPath = combinedPath;
            else
                targetPath = null;

            return new ResolvedFile() {
                IsResolved = !string.IsNullOrWhiteSpace(targetPath),
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
        protected abstract ResolvedFile DoResolvePath(string basePath, string pathToResolve);
    }
}
