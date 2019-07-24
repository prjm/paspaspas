namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     interface for reader input
    /// </summary>
    public interface IReaderInput {

        /// <summary>
        ///     create a new buffer source
        /// </summary>
        /// <returns></returns>
        IBufferSource CreateBufferSource();

        /// <summary>
        ///     file path
        /// </summary>
        string Path { get; }

        /// <summary>
        ///     buffer size
        /// </summary>
        int BufferSize { get; }
    }
}
