using System.IO;
using PasPasPas.Globals.Files;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     resolve something from the search path
    /// </summary>
    public abstract class SearchPathResolver : PathResolver {

        /// <summary>
        ///  parent option set
        /// </summary>
        private readonly OptionSet optionSet;

        /// <summary>
        ///     Create a new search path resolve
        /// </summary>
        /// <param name="options"></param>
        protected SearchPathResolver(OptionSet options) : base(options.Environment.StringPool)
            => optionSet = options;

        /// <summary>
        ///     resolve a file from the search path
        /// </summary>
        /// <param name="basePath">base path</param>
        /// <param name="pathToResolve">path to resolve</param>
        /// <returns>resolved file</returns>
        protected ResolvedFile ResolveFromSearchPath(FileReference basePath, FileReference pathToResolve) {
            string currentDirectory;

            if (basePath != null && !string.IsNullOrEmpty(basePath.Path)) {
                currentDirectory = Path.GetDirectoryName(Path.GetFullPath(basePath.Path));
            }
            else {
                currentDirectory = null;
            }

            var result = ResolveInDirectory(new FileReference(currentDirectory), pathToResolve);
            if (result.IsResolved) {
                return result;
            }

            foreach (var path in optionSet.PathOptions.SearchPaths) {
                result = ResolveInDirectory(path, pathToResolve);
                if (result.IsResolved) {
                    return result;
                }
            }

            return result;
        }

    }
}