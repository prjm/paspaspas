using PasPasPas.Infrastructure.Service;
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
    public abstract class PathResolver : IObjectBase {

        private Dictionary<ResolvedPathKey, string> resolvedPaths
            = new Dictionary<ResolvedPathKey, string>();

        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="services"></param>
        protected PathResolver(IObjectBase services) {
            ObjectBase = services;
        }

        /// <summary>
        ///     get services
        /// </summary>
        public ServiceProvider Services
            => ObjectBase.Services;

        /// <summary>
        ///     provided services
        /// </summary>
        protected IObjectBase ObjectBase { get; }

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
        ///     check if a given path exists in a directory
        /// </summary>
        /// <param name="currentDirectory">current directoy</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <param name="targetPath">target path</param>
        /// <returns></returns>
        protected bool ResolveInDirectory(string currentDirectory, string pathToResolve, out string targetPath) {
            var fileAccess = (IFileAccess)ObjectBase.Services.Resolve(StandardServices.FileAccessServiceClass, this);
            string combinedPath;

            if (!string.IsNullOrEmpty(currentDirectory))
                combinedPath = Path.Combine(currentDirectory, pathToResolve);
            else
                combinedPath = pathToResolve;

            if (fileAccess.FileExists(combinedPath))
                targetPath = combinedPath;
            else
                targetPath = null;

            return !string.IsNullOrEmpty(targetPath);
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
