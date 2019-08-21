using System;
using System.IO;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     resolver for resource files
    /// </summary>
    public class ResourceFilePathResolver : SearchPathResolver {

        /// <summary>
        ///     create a new path resolver
        /// </summary>
        /// <param name="options"></param>
        public ResourceFilePathResolver(IOptionSet options) : base(options) { }

        /// <summary>
        ///     resolve a resource path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override ResolvedFile DoResolvePath(IFileReference basePath, IFileReference pathToResolve) {

            if (pathToResolve.Path.StartsWith("*", StringComparison.Ordinal)) {

#if DESKTOP
                pathToResolve = new FileReference(pathToResolve.Path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path)));
#else
                pathToResolve = pathToResolve.CreateNewFileReference(pathToResolve.Path.Replace("*", Path.GetFileNameWithoutExtension(basePath.Path), StringComparison.OrdinalIgnoreCase));
#endif




            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(pathToResolve.Path))) {
                pathToResolve = pathToResolve.CreateNewFileReference(Path.ChangeExtension(pathToResolve.Path, ".res"));
            }

            return ResolveFromSearchPath(basePath, pathToResolve);
        }
    }
}
