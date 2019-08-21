using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;

namespace PasPasPas.Globals.Files {

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
            unchecked {
                result = result * 31 + BasePath.GetHashCode();
                result = result * 31 + PathToResolve.GetHashCode();
                return result;
            }
        }

    }

    /// <summary>
    ///     resolves paths for common scenarios
    /// </summary>
    public abstract class PathResolver : IPathResolver {

        private readonly IDictionary<ResolvedPathKey, ResolvedFile> resolvedPaths
            = new Dictionary<ResolvedPathKey, ResolvedFile>();

        /// <summary>
        ///     used string pool
        /// </summary>
        public IStringPool StringPool { get; }

        /// <summary>
        ///     input resolver
        /// </summary>
        public IInputResolver InputResolver { get; }

        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="pool">used string pool</param>
        /// <param name="resolver">resolver</param>
        protected PathResolver(IStringPool pool, IInputResolver resolver) {
            StringPool = pool ?? throw new ArgumentNullException(nameof(pool));
            InputResolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        ///     resolves a path and caches the result
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="fileName">path to resolve</param>
        /// <returns>resolved path</returns>
        public ResolvedFile ResolvePath(IFileReference basePath, IFileReference fileName) {

            if (basePath == null)
                throw new ArgumentNullException(nameof(basePath));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var key = new ResolvedPathKey() {
                BasePath = basePath,
                PathToResolve = fileName
            };

            if (!resolvedPaths.TryGetValue(key, out var result)) {
                result = DoResolvePath(basePath, fileName);
                resolvedPaths.Add(key, result);
            }

            return result;
        }

        /// <summary>
        ///     check if a given path exists in a directory
        /// </summary>
        /// <param name="currentDirectory">current directory</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns></returns>
        protected ResolvedFile ResolveInDirectory(IFileReference currentDirectory, IFileReference pathToResolve) {

            if (currentDirectory == null)
                throw new ArgumentNullException(nameof(currentDirectory));

            if (pathToResolve == null)
                throw new ArgumentNullException(nameof(pathToResolve));

            var combinedPath = currentDirectory.Append(pathToResolve);
            var isResolved = InputResolver.CanResolve(combinedPath);

            return new ResolvedFile(currentDirectory, pathToResolve, combinedPath, isResolved);
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
