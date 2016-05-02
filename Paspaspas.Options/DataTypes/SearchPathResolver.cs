using PasPasPas.Infrastructure.Input;
using System.IO;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     resolve something from the search path
    /// </summary>
    public abstract class SearchPathResolver : PathResolver {

        /// <summary>
        ///  parent option set
        /// </summary>
        private OptionSet optionSet;

        /// <summary>
        ///     Create a new search path resolve
        /// </summary>
        /// <param name="options"></param>
        protected SearchPathResolver(OptionSet options) : base(options) {
            optionSet = options;
        }

        /// <summary>
        ///     resolve a file from the search path
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns>resolved file</returns>
        protected ResolvedFile ResolveFromSearchPath(string basePath, string pathToResolve) {
            string targetPath;
            string currentDirectory;

            if (!string.IsNullOrEmpty(basePath)) {
                currentDirectory = Path.GetDirectoryName(Path.GetFullPath(basePath));
            }
            else {
                currentDirectory = null;
            }

            if (ResolveInDirectory(currentDirectory, pathToResolve, out targetPath)) {
                return targetPath;
            }

            foreach (var path in optionSet.PathOptions.SearchPaths) {
                if (ResolveInDirectory(path, pathToResolve, out targetPath)) {
                    return targetPath;
                }
            }

            return null;
        }

    }
}