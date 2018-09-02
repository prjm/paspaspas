namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     interface for buffer sources
    /// </summary>
    public interface IBufferSource {

        /// <summary>
        ///     get chars from the input
        /// </summary>
        /// <param name="target">target array</param>
        /// <param name="offset">char offset</param>
        /// <returns>number of chars read</returns>
        int GetContent(char[] target, long offset);

        /// <summary>
        ///     length in characters
        /// </summary>
        long Length { get; }
    }
}