using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.Files {

    internal class ResolvedPathKey {

        /// <summary>
        ///     base path
        /// </summary>
        public FileReference BasePath { get; set; }

        /// <summary>
        ///     path to resolve
        /// </summary>
        public FileReference PathToResolve { get; set; }

        /// <summary>
        ///     compare to keys for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var otherKey = obj as ResolvedPathKey;

            if (otherKey is null)
                return false;

            var basePathEquals = BasePath.Equals(otherKey.BasePath);
            var resolvePathEquals = PathToResolve.Equals(otherKey.PathToResolve);

            return basePathEquals && resolvePathEquals;
        }

        /// <summary>
        ///     compute a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
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
        ///     used string pool
        /// </summary>
        public StringPool StringPool { get; }

        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="pool">used string pool</param>
        protected PathResolver(StringPool pool)
            => StringPool = pool ?? throw new ArgumentNullException(nameof(pool));

        /// <summary>
        ///     resolves a path and caches the result
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns>resolved path</returns>
        public ResolvedFile ResolvePath(FileReference basePath, FileReference pathToResolve) {

            if (basePath == null)
                throw new ArgumentNullException(nameof(basePath));

            if (pathToResolve == null)
                throw new ArgumentNullException(nameof(pathToResolve));

            var key = new ResolvedPathKey() {
                BasePath = basePath,
                PathToResolve = pathToResolve
            };

            if (!resolvedPaths.TryGetValue(key, out var result)) {
                result = DoResolvePath(basePath, pathToResolve);
                resolvedPaths.Add(key, result);
            }

            return result;
        }

        /// <summary>
        ///     check if a given path exists in a directory
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="currentDirectory">current directory</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns></returns>
        protected ResolvedFile ResolveInDirectory(StringPool pool, FileReference currentDirectory, FileReference pathToResolve) {

            if (currentDirectory == null)
                throw new ArgumentNullException(nameof(currentDirectory));

            if (pathToResolve == null)
                throw new ArgumentNullException(nameof(pathToResolve));

            var combinedPath = currentDirectory.Append(pathToResolve);
            var isResolved = System.IO.File.Exists(combinedPath.Path);

            return new ResolvedFile(currentDirectory, pathToResolve, combinedPath, isResolved);
        }

        /// <summary>
        ///     resolve a path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected abstract ResolvedFile DoResolvePath(FileReference basePath, FileReference pathToResolve);
    }
}
