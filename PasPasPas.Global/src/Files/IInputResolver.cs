#nullable disable
using PasPasPas.Globals.Api;

namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     input file resolver
    /// </summary>
    public interface IInputResolver {

        /// <summary>
        ///     resolve an input file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="api"></param>
        /// <returns></returns>
        IReaderInput Resolve(IReaderApi api, IFileReference file);

        /// <summary>
        ///     test if a given file can be resolved
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool CanResolve(IFileReference file);

    }
}
