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
        private Tokenizer tokenizer;
        private readonly StackedFileReader input;
        private readonly ILogSource log;
        private readonly StaticEnvironment environment;

        /// <summary>
        ///     create a new tokenizer state
        /// </summary>
        /// <param name="parentTokenizer"></param>
        public TokenizerState(StaticEnvironment staticEnvironment, Tokenizer parentTokenizer, StackedFileReader currentInput, ILogSource logSource) {
            tokenizer = parentTokenizer;
            environment = staticEnvironment;
            log = logSource;
            input = currentInput;
            bufferHolder = PoolFactory.FetchStringBuilder(environment);
            buffer = bufferHolder.Data;
        }

        /// <summary>
        ///     current buffer length
        /// </summary>
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

        /// <summary>
        ///     start position
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        ///     static environment
        /// </summary>
        public StaticEnvironment Environment
        => environment;

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
            if (bufferHolder != null) {
                bufferHolder.Dispose();
                bufferHolder = null;
            }
        }

        /// <summary>
        ///     create an error message
        /// </summary>
        /// <param name="errorId">error id</param>
        public void Error(Guid errorId)
            => log.Error(errorId);

        public char LookAhead(int number = 1)
            => input.LookAhead(number);

        public char GetBufferCharAt(int index)
            => buffer[index];

        public string GetBufferContent()
            => StringPool.PoolString(buffer.ToString());

        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Append(result);
            return result;
        }

        public char PreviousChar()
            => input.PreviousChar();

    }

}