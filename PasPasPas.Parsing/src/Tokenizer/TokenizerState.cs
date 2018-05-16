using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Files;
using System.Text;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Globals.Runtime;
using System.Collections.Generic;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     state management for a tokenizer
    /// </summary>
    public class TokenizerState : IDisposable {

        private ObjectPool<List<char>>.PoolItem bufferHolder;
        private IList<char> buffer;
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
            get => buffer.Count;
            set {
                while (buffer.Count > value && buffer.Count > 0)
                    buffer.RemoveAt(buffer.Count - 1);
            }
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
        ///     convert a parsed literal to a char literal
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
        public ObjectPool<List<char>>.PoolItem FetchStringBuilder()
            => environment.CharListPool.Borrow();

        /// <summary>
        ///     append a char
        /// </summary>
        /// <param name="currentChar"></param>
        public void Append(char currentChar)
            => buffer.Add(currentChar);

        /// <summary>
        ///     tests if the buffer ends with a given char
        /// </summary>
        /// <param name="endSequence"></param>
        /// <returns></returns>
        public bool BufferEndsWith(char endSequence)
            => buffer.Count > 0 && buffer[buffer.Count - 1] == endSequence;

        /// <summary>
        ///     tests if the buffer ends with a given string
        /// </summary>
        /// <param name="endSequence"></param>
        /// <returns></returns>
        public bool BufferEndsWith(string endSequence) {

            if (buffer.Count < endSequence.Length)
                return false;

            var offset = buffer.Count - endSequence.Length;

            for (var i = 0; i < endSequence.Length; i++)
                if (buffer[offset + i] != endSequence[i])
                    return false;

            return true;
        }

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
        public ITypeReference ConvertRealLiteral(string literal)
            => environment.RealLiteralConverter.Convert(literal);

        /// <summary>
        ///     get the buffer content as pooled string
        /// </summary>
        /// <returns></returns>
        public string GetBufferContent()
            => environment.StringPool.PoolString(buffer);

        /// <summary>
        ///     fetch the next char
        /// </summary>
        /// <param name="append">if <c>true</c>, the character is appended to the buffer</param>
        /// <returns></returns>
        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Add(result);
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