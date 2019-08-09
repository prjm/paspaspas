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
        IReaderInput Resolve(FileReference file);

    }
}
