using System;
using System.Collections.Generic;
using System.Text;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     helper class to store file state
    /// </summary>
    internal class PartlyReadFile {

        /// <summary>
        ///     crete a new partly read file
        /// </summary>
        /// <param name="input"></param>
        internal PartlyReadFile(IParserInput input) {

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            InputFile = input;
            Active = true;
        }

        /// <summary>
        ///     flag, <c>true</c> if the file is active
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        ///     file to read
        /// </summary>
        public IParserInput InputFile { get; }

        /// <summary>
        ///     close file
        /// </summary>
        /// <param name="dispose">dispose</param>
        public void Close(bool dispose) {
            if (!Active)
                return;

            InputFile.Close();

            if (dispose)
                InputFile.Dispose();

            Active = false;
        }

        /// <summary>
        ///     reopen file
        /// </summary>
        public void Open() {
            if (Active)
                return;

            InputFile.Open();
            Active = true;
        }
    }

    /// <summary>
    ///     helper class for putback fragments
    /// </summary>
    internal class PutbackFragment : IFile {

        private readonly IFileReference path;

        /// <summary>
        ///     create a new putback fragment
        /// </summary>
        /// <param name="originalPath">original file path</param>
        /// <param name="putbackBuffer">character buffer</param>
        internal PutbackFragment(IFileReference originalPath, Stack<char> putbackBuffer) {

            if (originalPath == null)
                throw new ArgumentNullException(nameof(originalPath));

            if (putbackBuffer == null)
                throw new ArgumentNullException(nameof(putbackBuffer));

            path = originalPath;
            buffer = putbackBuffer;
            startPosition = putbackBuffer.Count;
        }

        /// <summary>
        ///     start position
        /// </summary>
        private readonly int startPosition;

        /// <summary>
        ///     putback chars
        /// </summary>
        private readonly Stack<char> buffer;

        /// <summary>
        ///     current file
        /// </summary>
        public IFileReference FilePath
            => path;

        /// <summary>
        ///     get another character
        /// </summary>
        /// <returns></returns>
        internal char Pop()
            => buffer.Pop();

        /// <summary>
        ///     check if the buffer is at eof
        /// </summary>
        /// <returns></returns>
        internal bool AtEof()
            => buffer.Count == startPosition;

        /// <summary>
        ///     putback a string
        /// </summary>
        /// <param name="valueToPutback"></param>
        internal void Putback(string valueToPutback) {
            if (string.IsNullOrEmpty(valueToPutback))
                throw new ArgumentNullException(nameof(valueToPutback));

            for (var charIndex = valueToPutback.Length - 1; charIndex >= 0; charIndex--) {
                buffer.Push(valueToPutback[charIndex]);
            }
        }

        /// <summary>
        ///     putback a string buffer
        /// </summary>
        /// <param name="charsToPutback"></param>
        internal void Putback(StringBuilder charsToPutback) {
            // REMOVE REMOVE
            for (var charIndex = charsToPutback.Length - 1; charIndex >= 0; charIndex--) {
                buffer.Push(charsToPutback[charIndex]);
            }
            charsToPutback.Clear();
        }

        /// <summary>
        ///     putback a single character
        /// </summary>
        /// <param name="valueToPutback"></param>
        internal void Putback(char valueToPutback) {
            buffer.Push(valueToPutback);
        }
    }

    /// <summary>
    ///     read from a combiniation of textfiles
    /// </summary>
    public class OldStackedFileReader : IDisposable {

        /// <summary>
        ///     files to read
        /// </summary>
        private readonly Stack<PartlyReadFile> files
            = new Stack<PartlyReadFile>();

        /// <summary>
        ///     putback fragments
        /// </summary>
        private readonly Stack<PutbackFragment> putbackFragments
            = new Stack<PutbackFragment>();

        /// <summary>
        ///     putback chars
        /// </summary>
        private readonly Stack<char> putbackChars
            = new Stack<char>();

        /// <summary>
        ///     test if end of input has reached
        /// </summary>
        public bool AtEof {
            get {
                RemoveEmptyFiles();
                return (putbackFragments.Count < 1) && (files.Count < 1);
            }
        }

        /// <summary>
        ///     currently read file path
        /// </summary>
        public IFileReference CurrentInputFile {
            get {
                if (putbackFragments.Count > 0)
                    return putbackFragments.Peek().FilePath;
                else if (files.Count > 0)
                    return files.Peek().InputFile.FilePath;
                else
                    return lastFilePath;
            }
        }

        /// <summary>
        ///     fetch the next char
        /// </summary>
        /// <param name="switchedInput"><c>true</c> if the input file switched</param>
        /// <returns>next input character or <c>\o</c> if at <c>eof</c></returns>
        public char FetchChar(out bool switchedInput) {
            char result;

            RemoveEmptyFiles();

            if (AtEof) {
                throw new InvalidOperationException();
            }

            if (putbackFragments.Count > 0) {
                return FetchCharFromFragment(out switchedInput);
            }

            PartlyReadFile file = files.Peek();
            result = file.InputFile.NextChar();

            if (file.InputFile.AtEof) {
                lastFilePath = file.InputFile.FilePath;
                file.Close(true);
                files.Pop();
                switchedInput = true;

                if (!AtEof) {
                    file = files.Peek();
                    file.Open();
                }

                return result;
            }

            switchedInput = false;
            return result;
        }

        private void RemoveEmptyFiles() {
            while (files.Count > 0 && files.Peek().InputFile.AtEof) {
                lastFilePath = files.Peek().InputFile.FilePath;
                files.Pop();
            }
        }

        private char FetchCharFromFragment(out bool switchedInput) {
            PutbackFragment fragment = putbackFragments.Peek();
            char result;
            result = fragment.Pop();
            if (fragment.AtEof()) {
                lastFilePath = fragment.FilePath;
                putbackFragments.Pop();
                RemoveEmptyFiles();

                if (putbackFragments.Count > 0)
                    switchedInput = !putbackFragments.Peek().FilePath.Equals(lastFilePath);
                else
                    switchedInput = files.Count < 1 || !files.Peek().InputFile.FilePath.Equals(lastFilePath);


                return result;
            }

            switchedInput = false;
            return result;
        }

        /// <summary>
        ///     put back an entire string
        /// </summary>
        /// <param name="valueToPutback"></param>
        /// <param name="file">source file</param>
        public void PutbackString(IFileReference file, string valueToPutback) {

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (string.IsNullOrEmpty(valueToPutback))
                throw new ArgumentNullException(nameof(valueToPutback));

            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(valueToPutback);
        }

        private PutbackFragment GetOrCreateFragment(IFileReference file) {
            PutbackFragment fragment;
            IFileReference currentFile = null;

            if (putbackFragments.Count > 0) {
                currentFile = putbackFragments.Peek().FilePath;
            }

            if (currentFile != null && file.Equals(currentFile)) {
                fragment = putbackFragments.Peek();
            }
            else {
                fragment = new PutbackFragment(file, putbackChars);
                putbackFragments.Push(fragment);
            }

            return fragment;
        }

        /// <summary>
        ///     put back a single char
        /// </summary>
        /// <param name="file"></param>
        /// <param name="valueToPutback"></param>
        public void PutbackChar(IFileReference file, char valueToPutback) {

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(valueToPutback);
        }

        /// <summary>
        ///     put back an entire stringbuffer
        /// </summary>
        /// <param name="buffer">buffer to put back</param>
        /// <param name="file">name of the file</param>
        public void PutbackStringBuffer(IFileReference file, StringBuilder buffer) {

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            PutbackFragment fragment = GetOrCreateFragment(file);
            fragment.Putback(buffer);
        }

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFile(IParserInput input) {

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            SuspendCurrentFile();
            files.Push(new PartlyReadFile(input));
        }

        private void SuspendCurrentFile() {
            if (files.Count < 1)
                return;

            PartlyReadFile file = files.Peek();
            file.Close(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        /// <summary>
        ///     last file path
        /// </summary>
        private IFileReference lastFilePath;

        /// <summary>
        ///     dispose redaers
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    while (files != null && files.Count > 0) {
                        PartlyReadFile file = files.Pop();
                        lastFilePath = file.InputFile.FilePath;
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
