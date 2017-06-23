using System;
using System.Text;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     encapsulation of current pattern continuation state
    /// </summary>
    public class ContinuationState {

        private bool switchedFile = false;

        /// <summary>
        ///     parsed token
        /// </summary>
        public Token Result { get; set; }

        /// <summary>
        ///     input buffer
        /// </summary>
        public StringBuilder Buffer { get; set; }

        /// <summary>
        ///     test if reading from the buffer is invalid
        /// </summary>
        public bool IsValid
            => !InputFile.AtEof;

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; set; }

        /// <summary>
        ///     current input length
        /// </summary>
        public int Length
             => Buffer.Length;

        /// <summary>
        ///     switched file flag
        /// </summary>
        public bool SwitchedFile
            => switchedFile;

        /// <summary>
        ///     file input
        /// </summary>
        public FileBufferItemOffset InputFile { get; internal set; }

        /// <summary>
        ///     tests if the buffer ends with a given string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EndsWith(string value)
            => Buffer.EndsWith(value);

        /// <summary>
        ///     finish tokenizing
        /// </summary>
        /// <param name="tokenId">generated token id</param>
        public void Finish(int tokenId) {
            Result.Kind = tokenId;
            Result.Value = Buffer.ToString();
        }

        /// <summary>
        ///     append a character
        /// </summary>
        public char FetchAndAppendChar() {
            char result;

            result = InputFile.Value;
            InputFile.NextChar();

            Buffer.Append(result);
            return result;
        }

        /// <summary>
        ///     clear state
        /// </summary>
        public void Clear() {
            switchedFile = false;
            Result = null;
        }

        /// <summary>
        ///     fetch a single character without appending it
        /// </summary>
        /// <returns></returns>
        public char FetchChar() {
            char result = InputFile.Value;
            InputFile.NextChar();
            return result;
        }

        /// <summary>
        ///     putback a character
        /// </summary>
        /// <param name="nextChar">character to putback</param>
        /// <param name="switchState">switch state</param>
        public void Putback(char nextChar, bool switchState) {
            switchedFile = switchState;
            InputFile.PreviousChar();
        }

        /// <summary>
        ///     append a character
        /// </summary>
        /// <param name="currentChar"></param>
        internal void AppendChar(char currentChar) {
            Buffer.Append(currentChar);
        }

        internal void PutbackBuffer() {
            for (int i = 0; i < Buffer.Length; i++)
                InputFile.PreviousChar();
            Buffer.Clear();
        }

        internal void Finish(int tokenKind, string value) {
            Result.Kind = tokenKind;
            Result.Value = value;
        }

        internal void Error(Guid messageId, params object[] messageData) {
            Log.ProcessMessage(new LogMessage(MessageSeverity.Error, TokenizerBase.TokenizerLogMessage, messageId, messageData));
        }
    }
}