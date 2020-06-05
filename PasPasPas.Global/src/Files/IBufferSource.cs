#nullable disable
using System;

namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     interface for buffer sources
    /// </summary>
    public interface IBufferSource : IDisposable {

        /// <summary>
        ///     get chars from the input
        /// </summary>
        /// <param name="target">target array</param>
        /// <param name="offset">char offset</param>
        /// <param name="bufferSize">buffer size</param>
        /// <returns>number of chars read</returns>
        int GetContent(char[] target, int bufferSize, long offset);

        /// <summary>
        ///     length in characters
        /// </summary>
        long Length { get; }
    }
}