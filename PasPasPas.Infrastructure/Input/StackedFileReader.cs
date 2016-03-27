using System;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     helper class to store file state
    /// </summary>
    internal class PartlyReadFile {

        /// <summary>
        ///     flag, <c>true</c> if the file is active
        /// </summary>
        public bool Active { get; internal set; }

        /// <summary>
        ///     file to read
        /// </summary>
        public IParserInput InputFile { get; set; }

        /// <summary>
        ///     close file
        /// </summary>
        /// <param name="dispose">dispose</param>
        public void Close(bool dispose) {
            if (!Active) return;
            InputFile.Close();
            if (dispose)
                InputFile.Dispose();
            Active = false;
        }

        /// <summary>
        ///     reopen file
        /// </summary>
        public void Open() {
            if (Active) return;
            InputFile.Open();
            Active = true;
        }
    }

    /// <summary>
    ///     
    /// </summary>
    public class StackedFileReader : IDisposable {

        /// <summary>
        ///     files to read
        /// </summary>
        private Stack<PartlyReadFile> files
            = new Stack<PartlyReadFile>();

        /// <summary>
        ///     putback chars
        /// </summary>
        private Stack<char> putbackBuffer
    = new Stack<char>(256);

        /// <summary>
        ///     test if end of input has reached
        /// </summary>
        public bool AtEof
            => (putbackBuffer.Count < 1) && (files.Count < 1);

        /// <summary>
        ///     currently read file
        /// </summary>
        public IFile CurrentFile
        {
            get
            {
                if (files.Count > 0)
                    return files.Peek().InputFile;
                else
                    return null;
            }
        }

        /// <summary>
        ///     fetch the next char
        /// </summary>
        /// <returns></returns>
        public char FetchChar() {
            if (AtEof)
                return '\0';

            if (putbackBuffer.Count > 0)
                return putbackBuffer.Pop();

            var file = files.Peek();
            var readChar = file.InputFile.NextChar();

            if (file.InputFile.AtEof) {
                file.Close(true);
                files.Pop();

                if (!AtEof) {
                    file = files.Peek();
                    file.Open();
                }
            }

            return readChar;
        }

        /// <summary>
        ///     put back an entire string
        /// </summary>
        /// <param name="valueToPutback"></param>
        public void PutbackString(string valueToPutback) {
            for (int charIndex = valueToPutback.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(valueToPutback[charIndex]);
            }
        }

        /// <summary>
        ///     put back an entire stringbuffer
        /// </summary>
        /// <param name="buffer"></param>
        public void PutbackStringBuffer(StringBuilder buffer) {
            for (int charIndex = buffer.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(buffer[charIndex]);
            }
            buffer.Clear();
        }



        /// <summary>
        ///     putback a single char
        /// </summary>
        /// <param name="valueToPutback"></param>
        public void PutbackChar(char valueToPutback) {
            putbackBuffer.Push(valueToPutback);
        }

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFile(IParserInput input) {
            SuspendCurrentFile();
            AppendFile(input);
        }

        private void AppendFile(IParserInput input) {
            files.Push(new PartlyReadFile() {
                InputFile = input,
                Active = true,
            });
        }

        private void SuspendCurrentFile() {
            if (files.Count < 1)
                return;

            var file = files.Peek();
            file.Close(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        /// <summary>
        ///     dispose redaers
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    while (files != null && files.Count > 0) {
                        var file = files.Pop();
                        file.Close(true);
                    }
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     diispose readers
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
