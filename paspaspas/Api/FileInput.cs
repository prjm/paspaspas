using System;
using System.Collections.Generic;
using System.IO;

namespace PasPasPas.Api {

    /// <summary>
    ///     file based input for the parser
    /// </summary>
    public class FileInput : IParserInput, IDisposable {

        private Stack<char> putbackChars = new Stack<char>();
        private StreamReader reader = null;

        /// <summary>
        ///     input file
        /// </summary>
        public string FileName { get; set; }

        private StreamReader Reader
        {
            get
            {
                if (reader != null)
                    return reader;

                reader = new StreamReader(FileName);
                return reader;
            }
        }

        /// <summary>
        ///     check if the reader is at the end of file
        /// </summary>
        /// <returns><c>true</c> if the end of file is reached</returns>
        public bool AtEof()
            => (Reader.EndOfStream) && (putbackChars.Count < 1);

        /// <summary>
        ///     read the next char
        /// </summary>
        /// <returns></returns>
        public char NextChar() {
            if (putbackChars.Count > 0)
                return putbackChars.Pop();
            else
                return (char)Reader.Read();
        }


        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        ///     dispose input read
        /// </summary>
        /// <param name="disposing"><c>true</c> if ressources are disposed</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (reader != null)
                        reader.Dispose();
                }

                reader = null;
                disposedValue = true;
            }
        }

        /// <summary>
        ///     Dispose the input string
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        /// <summary>
        ///     putback a given character
        /// </summary>
        /// <param name="valueToPutback"></param>
        public void Putback(char valueToPutback) {
            putbackChars.Push(valueToPutback);
        }

        #endregion

    }
}
