﻿#nullable disable
using System;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     new string source
    /// </summary>
    public sealed class StringBufferSource : IBufferSource {

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
        /// <param name="bufferSize">buffer size</param>
        /// <returns>number of characters</returns>
        public int GetContent(char[] target, int bufferSize, long offset) {
            var charsToCopy = (int)Math.Max(0, Math.Min(bufferSize, Length - offset));

            if (offset < 0 || offset >= Length)
                return 0;

            if (charsToCopy > 0)
                bufferContent.CopyTo((int)offset, target, 0, charsToCopy);

            return charsToCopy;
        }

        /// <summary>
        ///     dispose this buffer source
        /// </summary>
        public void Dispose() { }

    }
}
