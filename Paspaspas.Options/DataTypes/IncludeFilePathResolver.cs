using PasPasPas.Infrastructure.Input;
using System.IO;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path resolver for file includes
    /// </summary>
    public class IncludeFilePathResolver : PathResolver {

        /// <summary>
        ///  parent option set
        /// </summary>
        private OptionSet optionSet;

        /// <summary>
        ///     create a new include file pat resolve
        /// </summary>
        /// <param name="options"></param>
        public IncludeFilePathResolver(OptionSet options)
            : base(options) {
            optionSet = options;
        }

        /// <summary>
        ///     resolve a path 
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override string DoResolvePath(string basePath, string pathToResolve) {
            string fullPath = Path.GetFullPath(basePath);
            string currentDirectory = Path.GetDirectoryName(basePath);
            string targetPath;

            if (ResolveInDirectory(currentDirectory, pathToResolve, out targetPath)) {
                return targetPath;
            }

            foreach (var path in optionSet.PathOptions.SearchPaths) {
                if (ResolveInDirectory(currentDirectory, pathToResolve, out targetPath)) {
                    return targetPath;
                }
            }

            return null;
        }
    }
}