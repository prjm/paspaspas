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

    internal class PutbackFragment : IFile {

        private readonly IFileReference path;

        internal PutbackFragment(IFileReference originalPath) {
            path = originalPath;
        }

        /// <summary>
        ///     putback chars
        /// </summary>
        private Stack<char> putbackBuffer
            = new Stack<char>(16);

        public IFileReference FilePath
            => path;

        internal char Pop() => putbackBuffer.Pop();

        internal bool AtEof() => putbackBuffer.Count == 0;

        internal void Putback(string valueToPutback) {
            for (int charIndex = valueToPutback.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(valueToPutback[charIndex]);
            }
        }

        internal void Putback(StringBuilder buffer) {
            for (int charIndex = buffer.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(buffer[charIndex]);
            }
            buffer.Clear();
        }

        internal void Putback(char valueToPutback) {
            putbackBuffer.Push(valueToPutback);
        }
    }

    /// <summary>
    ///     read from a combiniation of textfiles
    /// </summary>
    public class StackedFileReader : IDisposable {

        /// <summary>
        ///     files to read
        /// </summary>
        private Stack<PartlyReadFile> files
            = new Stack<PartlyReadFile>();

        /// <summary>
        ///     putback fragments
        /// </summary>
        private Stack<PutbackFragment> putbackFragments
            = new Stack<PutbackFragment>();

        /// <summary>
        ///     test if end of input has reached
        /// </summary>
        public bool AtEof
            => (putbackFragments.Count < 1) && (files.Count < 1);

        /// <summary>
        ///     currently read file
        /// </summary>
        public IFile CurrentFile
        {
            get
            {
                if (putbackFragments.Count > 0)
                    return putbackFragments.Peek();
                else if (files.Count > 0)
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

            if (putbackFragments.Count > 0) {
                var fragment = putbackFragments.Peek();
                char result = fragment.Pop();
                if (fragment.AtEof())
                    putbackFragments.Pop();
                return result;
            }

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
        /// <param name="file">source file</param>
        public void PutbackString(IFile file, string valueToPutback) {
            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(valueToPutback);
        }

        private PutbackFragment GetOrCreateFragment(IFile file) {
            PutbackFragment fragment;
            IFileReference newFile = file.FilePath;
            IFileReference currentFile = null;

            if (putbackFragments.Count > 0) {
                currentFile = putbackFragments.Peek().FilePath;
            }

            if (currentFile != null && newFile.Equals(currentFile.Path)) {
                fragment = putbackFragments.Peek();
            }
            else {
                fragment = new PutbackFragment(file.FilePath);
                putbackFragments.Push(fragment);
            }

            return fragment;
        }

        /// <summary>
        ///     put back a single char
        /// </summary>
        /// <param name="file"></param>
        /// <param name="valueToPutback"></param>
        public void PutbackChar(IFile file, char valueToPutback) {
            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(valueToPutback);
        }

        /// <summary>
        ///     put back an entire stringbuffer
        /// </summary>
        /// <param name="buffer">buffer to put back</param>
        /// <param name="file">name of the file</param>
        public void PutbackStringBuffer(IFile file, StringBuilder buffer) {
            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(buffer);
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
