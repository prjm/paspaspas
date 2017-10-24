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
        ///     referenced file
        /// </summary>
        private IFileReference file;

        /// <summary>
        ///     create a new file buffer item
        /// </summary>
        /// <param name="fileName">file name</param>
        public FileBufferItem(IFileReference fileName)
            => file = fileName;

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
        /// <param name="nullOnInvalidOffset">return <c>\0</c> on a invalid offset</param>
        /// <returns>chat the offset</returns>
        public char CharAt(int offset, bool nullOnInvalidOffset) {
            if (offset < 0 || offset >= data.Length)
                if (nullOnInvalidOffset)
                    return '\0';
                else
                    throw new IndexOutOfRangeException($"Offset {offset} out of range.");

            return data[offset];
        }

        /// <summary>
        ///     filename
        /// </summary>
        public IFileReference File
            => file;
    }
}