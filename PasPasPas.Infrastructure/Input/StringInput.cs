using System;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     parser input for <c>strings</c>
    /// </summary>
    public class StringInput : IParserInput {

        /// <summary>
        ///     input data
        /// </summary>
        private readonly string input;

        /// <summary>
        ///     virtual path
        /// </summary>
        private readonly IFileReference path;

        /// <summary>
        ///     input position
        /// </summary>
        private int position = 0;

        /// <summary>
        ///     creates a new string input
        /// </summary>
        /// <param name="inputText">string input</param>
        /// <param name="virtualPath">virtual path</param>
        public StringInput(string inputText, IFileReference virtualPath) {

            if (virtualPath == null)
                throw new ArgumentNullException(nameof(virtualPath));

            if (inputText == null)
                throw new ArgumentNullException(nameof(inputText));

            input = inputText;
            path = virtualPath;
        }

        /// <summary>
        ///     check for eof
        /// </summary>
        public bool AtEof
            => position >= input.Length;

        /// <summary>
        ///     retrieve current position
        /// </summary>
        public long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = (int)value;
            }
        }

        /// <summary>
        ///     path of this input
        /// </summary>
        public IFileReference FilePath
            => path;

        /// <summary>
        ///     do nothing
        /// </summary>
        public void Close() {
            // do nothing
        }

        /// <summary>
        ///     next char
        /// </summary>
        /// <returns></returns>
        public char NextChar() {
            char result = input[position];
            position++;
            return result;
        }

        /// <summary>
        ///     open the file
        /// </summary>
        public void Open() {
            // ...
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        /// <summary>
        ///     dispose data
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose input
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
