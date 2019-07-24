using System;
using System.IO;
using PasPasPas.Globals.Files;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;

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
        protected override ResolvedFile DoResolvePath(FileReference basePath, FileReference pathToResolve) {

            if (pathToResolve.Path.StartsWith("*", StringComparison.Ordinal)) {

#if DESKTOP
                pathToResolve = new FileReference(pathToResolve.Path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path)));
#else
                pathToResolve = new FileReference(pathToResolve.Path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path), StringComparison.OrdinalIgnoreCase));
#endif




            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(pathToResolve.Path))) {
                pathToResolve = new FileReference(Path.ChangeExtension(pathToResolve.Path, ".res"));
            }

            return ResolveFromSearchPath(basePath, pathToResolve);
        }
    }
}
