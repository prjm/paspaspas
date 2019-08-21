using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     resolve files which are directly linked
    /// </summary>
    public class LinkedInputFileResolver : SearchPathResolver {


        /// <summary>
        ///        create a new file resolver
        /// </summary>
        /// <param name="parentOptions"></param>
        public LinkedInputFileResolver(IOptionSet parentOptions) : base(parentOptions) {
        }

        /// <summary>
        ///     resolve a linked input file
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override ResolvedFile DoResolvePath(IFileReference basePath, IFileReference pathToResolve)
            => ResolveFromSearchPath(basePath, pathToResolve);
    }
}