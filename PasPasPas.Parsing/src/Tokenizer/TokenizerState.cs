using System;
using System.Text;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     state management for a tokenizer
    /// </summary>
    public sealed class TokenizerState : IDisposable {

        private IPoolItem<StringBuilder> bufferHolder;

        private StringBuilder buffer;
        private readonly TokenizerBase tokenizer;
        private readonly IStackedFileReader input;
        private readonly ILogSource log;

        /// <summary>
        ///     create a new tokenizer state
        /// </summary>
        /// <param name="parentTokenizer"></param>
        /// <param name="currentInput">current input file</param>
        /// <param name="logSource">log source</param>
        /// <param name="parserEnvironment">parser environment</param>
        public TokenizerState(IParserEnvironment parserEnvironment, TokenizerBase parentTokenizer, IStackedFileReader currentInput, ILogSource logSource) {
            tokenizer = parentTokenizer;
            log = logSource;
            input = currentInput;
            Environment = parserEnvironment;
            bufferHolder = FetchStringBuilder();
            buffer = bufferHolder.Item;
            RuntimeValues = Environment.Runtime;
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
            => input == null || input.AtEof;

        /// <summary>
        ///     get the current value
        /// </summary>
        public char CurrentCharacter
            => input.Value;

        /// <summary>
        ///     get the current position
        /// </summary>
        public long CurrentPosition
            => input.Position;

        /// <summary>
        ///     start position
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        ///     get a pooled string buffer
        /// </summary>
        /// <returns>an item of the string builder pool</returns>
        public IPoolItem<StringBuilder> FetchStringBuilder()
            => Environment.StringBuilderPool.Borrow();

        /// <summary>
        ///     append a char
        /// </summary>
        /// <param name="currentChar"></param>
        public void Append(char currentChar)
            => buffer.Append(currentChar);

        /// <summary>
        ///     tests if the buffer ends with a given char
        /// </summary>
        /// <param name="endSequence"></param>
        /// <returns></returns>
        public bool BufferEndsWith(char endSequence)
            => buffer.EndsWith(endSequence);

        /// <summary>
        ///     tests if the buffer ends with a given string
        /// </summary>
        /// <param name="endSequence"></param>
        /// <returns></returns>
        public bool BufferEndsWith(string endSequence)
           => buffer.EndsWith(endSequence);

        /// <summary>
        ///     clear buffer content
        /// </summary>
        public void Clear()
            => buffer.Clear();

        /// <summary>
        ///     dispose this pooled object
        /// </summary>
        public void Dispose() {
            buffer = null;
            if (bufferHolder != null) {
                bufferHolder.Dispose();
                bufferHolder = null;
            }
        }

        /// <summary>
        ///     parse an integer literal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueParser"></param>
        /// <returns></returns>
        public ITypeReference ParserLiteral(string value, LiteralParserKind valueParser) {

            switch (valueParser) {

                case LiteralParserKind.HexNumbers:
                    return Environment.HexNumberParser.Parse(value);

                case LiteralParserKind.IntegerNumbers:
                    return Environment.IntegerParser.Parse(value);

            }

            throw new InvalidOperationException();
        }

        /// <summary>
        ///     create an error message
        /// </summary>
        /// <param name="errorId">error id</param>
        public void Error(uint errorId)
            => log.LogError(errorId);

        /// <summary>
        ///     lookahead for input char
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public char LookAhead(int number = 1)
            => input.LookAhead(number);

        /// <summary>
        ///     gets a char out from the buffer
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char GetBufferCharAt(int index)
            => buffer[index];

        /// <summary>
        ///     converts a real literal
        /// </summary>
        /// <param name="literal">literal value</param>
        /// <returns></returns>
        public ITypeReference ConvertRealLiteral(string literal)
            => Environment.RealLiteralConverter.Parse(literal);

        /// <summary>
        ///     get the buffer content as pooled string
        /// </summary>
        /// <returns></returns>
        public string GetBufferContent()
            => Environment.StringPool.PoolString(buffer);

        /// <summary>
        ///     fetch the next char
        /// </summary>
        /// <param name="append">if <c>true</c>, the character is appended to the buffer</param>
        /// <returns></returns>
        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Append(result);
            return result;
        }

        /// <summary>
        ///     parsing environment
        /// </summary>
        public IParserEnvironment Environment { get; }

        /// <summary>
        ///     constant value provider
        /// </summary>
        public IRuntimeValueFactory RuntimeValues { get; }

        /// <summary>
        ///     move one char backwards
        /// </summary>
        /// <returns></returns>
        public char PreviousChar()
            => input.PreviousChar();

    }

}