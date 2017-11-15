using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Files;
using System.Text;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

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
        private readonly IParserEnvironment environment;

        /// <summary>
        ///     create a new tokenizer state
        /// </summary>
        /// <param name="parentTokenizer"></param>
        /// <param name="currentInput">current input file</param>
        /// <param name="logSource">log source</param>
        /// <param name="parserEnvironment">parser environment</param>
        public TokenizerState(IParserEnvironment parserEnvironment, Tokenizer parentTokenizer, StackedFileReader currentInput, ILogSource logSource) {
            tokenizer = parentTokenizer;
            log = logSource;
            input = currentInput;
            environment = parserEnvironment;
            bufferHolder = FetchStringBuilder();
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

        public object ConvertCharLiteral(object parsedValue)
            => environment.CharLiteralConverter.Convert(parsedValue);

        /// <summary>
        ///     start position
        /// </summary>
        public int StartPosition { get; set; }

        public ObjectPool<StringBuilder>.PoolItem FetchStringBuilder()
            => environment.StringBuilderPool.Borrow();

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

        public object ParserLiteral(string value, LiteralParserKind valueParser) {
            switch (valueParser) {

                case LiteralParserKind.HexNumbers:
                    return environment.HexNumberParser.Parse(value);

                case LiteralParserKind.IntegerNumbers:
                    return environment.IntegerParser.Parse(value);

                default:
                    throw new InvalidOperationException();
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

        public object ConvertRealLiteral(object digits, object decimals, bool minus, object exp)
            => environment.RealLiteralConverter.Convert(digits, decimals, minus, exp);

        public string GetBufferContent()
            => environment.StringPool.PoolString(buffer.ToString());

        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Append(result);
            return result;
        }

        public IParserEnvironment Environment
            => environment;

        public char PreviousChar()
            => input.PreviousChar();

    }

}