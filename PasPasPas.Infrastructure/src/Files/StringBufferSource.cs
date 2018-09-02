using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     new string buffer
    /// </summary>
    public class StringBufferSource : IBufferSource {

        private readonly string bufferContent;

        /// <summary>
        ///     create a new string buffer source
        /// </summary>
        /// <param name="bufferValue">content of this buffer</param>
        public StringBufferSource(string bufferValue)
            => bufferContent = bufferValue;

        /// <summary>
        ///     string length
        /// </summary>
        public long Length =>
            bufferContent.Length;

        /// <summary>
        ///     get buffer content
        /// </summary>
        /// <param name="target">target array</param>
        /// <param name="offset">start offset</param>
        /// <returns>number of characters</returns>
        public int GetContent(char[] target, long offset) {

            var charsToCopy = (int)Math.Max(0, Math.Min(target.Length, Length - offset));
            var remainingChars = Math.Max(0, target.Length - charsToCopy);

            if (offset < 0 || offset >= Length)
                return 0;

            if (charsToCopy > 0)
                bufferContent.CopyTo((int)offset, target, 0, charsToCopy);

            return charsToCopy;
        }
    }
}
