using System;
using System.Collections.Generic;

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

            if (otherKey == null) return false;

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

        private Dictionary<ResolvedPathKey, string> resolvedPaths
            = new Dictionary<ResolvedPathKey, string>();

        /// <summary>
        ///     resolves a path and caches the result
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns>resolved path</returns>
        public string ResolvePath(string basePath, string pathToResolve) {
            var key = new ResolvedPathKey() {
                BasePath = basePath,
                PathToResolve = pathToResolve
            };

            string result;

            if (!resolvedPaths.TryGetValue(key, out result)) {
                result = DoResolvePath(basePath, pathToResolve);
                resolvedPaths.Add(key, result);
            }

            return result;
        }

        /// <summary>
        ///     resolve a path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected abstract string DoResolvePath(string basePath, string pathToResolve);
    }
}
