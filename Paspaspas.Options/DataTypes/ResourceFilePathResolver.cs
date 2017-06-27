using PasPasPas.Infrastructure.Input;
using PasPasPas.Options.Bundles;
using System;
using System.IO;
using PasPasPas.Infrastructure.Files;

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

            if (pathToResolve.Path.StartsWith("*", StringComparison.Ordinal)) {
                pathToResolve = Files.ReferenceToFile(pathToResolve.Path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path)));
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(pathToResolve.Path))) {
                pathToResolve = Files.ReferenceToFile(Path.ChangeExtension(pathToResolve.Path, ".res"));
            }

            return ResolveFromSearchPath(basePath, pathToResolve);
        }
    }
}
