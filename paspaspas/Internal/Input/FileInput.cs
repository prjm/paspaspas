using System;
using System.IO;

namespace PasPasPas.Internal.Input {

    /// <summary>
    ///     file based input for the parser
    /// </summary>
    public class FileInput : InputBase, IDisposable {

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
        ///     test if end of file is reached
        /// </summary>
        protected override bool IsSourceAtEof
            => Reader.EndOfStream;

        /// <summary>
        ///     get the next input characterfrom trhefile
        /// </summary>
        /// <returns></returns>
        protected override char NextCharFromSource()
            => (char)Reader.Read();

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

        #endregion

    }
}
