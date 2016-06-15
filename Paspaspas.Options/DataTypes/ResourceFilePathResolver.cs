using PasPasPas.Infrastructure.Input;
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
        protected override ResolvedFile DoResolvePath(IFileReference basePath, IFileReference pathToResolve) {
            var path = pathToResolve.Path;

            if (path.StartsWith("*", StringComparison.Ordinal)) {
                path = path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path));
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(pathToResolve.Path))) {
                pathToResolve = new FileReference(Path.ChangeExtension(pathToResolve.Path, ".res"));
            }

            return ResolveFromSearchPath(basePath, pathToResolve);
        }
    }
}
