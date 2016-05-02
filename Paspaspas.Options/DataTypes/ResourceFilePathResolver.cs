using PasPasPas.Options.Bundles;
using System;
using System.IO;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     resolver for resource files
    /// </summary>
    public class ResourceFilePathResolver : SearchPathResolver {

        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="options"></param>
        public ResourceFilePathResolver(OptionSet options) : base(options) { }

        /// <summary>
        ///     resolve a resource path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override string DoResolvePath(string basePath, string pathToResolve) {

            if (pathToResolve.StartsWith("*", StringComparison.Ordinal)) {
                pathToResolve = pathToResolve.Replace("*", Path.GetFileNameWithoutExtension(basePath));
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(pathToResolve))) {
                pathToResolve = Path.ChangeExtension(pathToResolve, ".res");
            }

            return ResolveFromSearchPath(basePath, pathToResolve);
        }
    }
}
