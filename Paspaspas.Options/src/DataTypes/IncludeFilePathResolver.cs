using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path resolver for file includes
    /// </summary>
    public class IncludeFilePathResolver : SearchPathResolver {

        /// <summary>
        ///     create a new include file pat resolve
        /// </summary>
        /// <param name="options"></param>
        public IncludeFilePathResolver(OptionSet options)
            : base(options) {
        }

        /// <summary>
        ///     resolve a path 
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override ResolvedFile DoResolvePath(FileReference basePath, FileReference pathToResolve)
            => ResolveFromSearchPath(basePath, pathToResolve);

    }
}