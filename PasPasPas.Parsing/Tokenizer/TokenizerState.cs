using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Files;
using System.Text;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer {


    /// <summary>
    ///     state management for a tokenizer
    /// </summary>
    public class TokenizerState : IDisposable {

        private ObjectPool<StringBuilder>.PoolItem bufferHolder;
        private StringBuilder buffer;
        private TokenizerBase tokenizer;
        private readonly StackedFileReader input;
        private readonly ILogSource log;

        /// <summary>
        ///     create a new tokenizer state
        /// </summary>
        /// <param name="parentTokenizer"></param>
        internal TokenizerState(TokenizerBase parentTokenizer, StackedFileReader currentInput, ILogSource logSource) {
            tokenizer = parentTokenizer;
            log = logSource;
            input = currentInput;
            bufferHolder = PoolFactory.FetchStringBuilder();
            buffer = bufferHolder.Data;
        }

        public int Length {
            get => buffer.Length;
            set => buffer.Length = value;
        }

        /// <summary>
        ///     check if the end of the current input file is reached
        /// </summary>
        public bool AtEof
            => input == null || input.CurrentFile.AtEof;

        /// <summary>
        ///     get the current value
        /// </summary>
        public char CurrentCharacter
            => input.Value;

        /// <summary>
        ///     get the current position
        /// </summary>
        public int CurrentPosition
            => input.Position;

        public void Append(char currentChar)
            => buffer.Append(currentChar);

        public bool BufferEndsWith(char endSequence)
            => buffer.EndsWith(endSequence);

        public bool BufferEndsWith(string endSequence)
            => buffer.EndsWith(endSequence);

        public void Clear()
            => buffer.Clear();

        public void Dispose() {
            buffer = null;
            bufferHolder.Dispose();
            bufferHolder = null;
        }

        public void Error(Guid errorId)
            => log.Error(errorId);

        public char GetBufferCharAt(int index)
            => buffer[index];

        public string GetBufferContent()
            => buffer.ToString();

        public bool KeepTokenValue(int tokenId)
            => tokenizer.KeepTokenValue(tokenId);

        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Append(result);
            return result;
        }

        public bool PrepareNextToken() {
            var file = input.CurrentFile;

            while (file != null && file.AtEof)
                file = input.FinishCurrentFile();

            return file != null;
        }


        public char PreviousChar()
            => input.PreviousChar();

        public void StartBufferWith(char startValue) {
            buffer.Clear();
            buffer.Append(startValue);
        }
    }

}