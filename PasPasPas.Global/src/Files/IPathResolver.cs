#nullable disable
namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     path resolver
    /// </summary>
    public interface IPathResolver {

        /// <summary>
        ///     resolve a file
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        ResolvedFile ResolvePath(IFileReference basePath, IFileReference fileName);
    }
}
