using System;
using System.Text;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     item of a file buffer
    /// </summary>
    public class FileBufferItem {

        private readonly StringBuilder data
            = new StringBuilder();

        /// <summary>
        ///     file content
        /// </summary>
        public StringBuilder Data
            => data;

        /// <summary>
        ///     content length (in chars)
        /// </summary>
        public int Length
            => data.Length;

        /// <summary>
        ///     get a char at a given value
        /// </summary>
        /// <param name="offset"></param>
        /// <returns>chat the offset</returns>
        public char CharAt(int offset) {
            if (offset < 0 || offset >= data.Length)
                ExceptionHelper.IndexOutOfRange(offset);
            return data[offset];
        }
    }
}