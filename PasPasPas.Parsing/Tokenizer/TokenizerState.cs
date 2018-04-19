using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Files;
using System.Text;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Global.Runtime;

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
        private readonly IRuntimeValueFactory constants;

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
            constants = environment.ConstantValues;
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
        ///     convert a parsed litaral to a char literal
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        public object ConvertCharLiteral(object parsedValue)
            => environment.CharLiteralConverter.Convert(parsedValue);

        /// <summary>
        ///     start position
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        ///     get a pooled string buffer
        /// </summary>
        /// <returns></returns>
        public ObjectPool<StringBuilder>.PoolItem FetchStringBuilder()
            => environment.StringBuilderPool.Borrow();

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
        public IValue ParserLiteral(string value, LiteralParserKind valueParser) {

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
        public IValue ConvertRealLiteral(string literal)
            => environment.RealLiteralConverter.Convert(literal);

        /// <summary>
        ///     get the buffer content as pooled string
        /// </summary>
        /// <returns></returns>
        public string GetBufferContent()
            => environment.StringPool.PoolString(buffer.ToString());

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
        public IParserEnvironment Environment
            => environment;

        /// <summary>
        ///     constant value provider
        /// </summary>
        public IRuntimeValueFactory Constants
            => constants;

        /// <summary>
        ///     move one char backwards
        /// </summary>
        /// <returns></returns>
        public char PreviousChar()
            => input.PreviousChar();

    }

}