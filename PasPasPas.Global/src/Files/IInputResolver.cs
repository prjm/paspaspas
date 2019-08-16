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
        /// <returns></returns>
        IReaderInput Resolve(IReaderApi api, FileReference file);

        /// <summary>
        ///     test if a given file can be resolved
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool CanResolve(FileReference file);

    }
}
