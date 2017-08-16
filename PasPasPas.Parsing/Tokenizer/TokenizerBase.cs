using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;
using System.Text;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase : ITokenizer {

        internal class TokenizerState : ITokenizerState {

            private TokenizerBase tokenizer;

            private readonly StringBuilder buffer
                = new StringBuilder(100);

            private readonly StackedFileReader input;

            private readonly ILogSource log;

            /// <summary>
            ///     create a new tokenizer state
            /// </summary>
            /// <param name="tokenizer"></param>
            internal TokenizerState(TokenizerBase tokenizer, StackedFileReader input, ILogSource log) {
                this.tokenizer = tokenizer;
                this.log = log;
                this.input = input;
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

        /// <summary>
        ///     message group for tokenizer logs
        /// </summary>
        public static readonly Guid TokenizerLogMessage
            = new Guid(new byte[] { 0xc6, 0x78, 0xdb, 0x93, 0x84, 0x6a, 0xff, 0x47, 0xaf, 0xe2, 0x82, 0xb3, 0xb3, 0x7f, 0x33, 0x26 });
        /* {93db78c6-6a84-47ff-afe2-82b3b37f3326} */

        /// <summary>
        ///     message: unexpected token
        /// </summary>    
        public static readonly Guid UnexpectedCharacter
            = new Guid(new byte[] { 0xd0, 0x79, 0xa5, 0xd0, 0x34, 0xa6, 0xba, 0x4c, 0x9d, 0x6, 0xc, 0x69, 0xde, 0xa6, 0x9e, 0xb });
        /* {d0a579d0-a634-4cba-9d06-0c69dea69e0b} */

        /// <summary>
        ///     message: unexptected end of token
        /// </summary>
        /// <remarks>
        ///     data: expected-token-end sequence
        /// </remarks>
        public static readonly Guid UnexpectedEndOfToken
            = new Guid(new byte[] { 0x7d, 0xd3, 0xfc, 0xf2, 0x4a, 0x89, 0x71, 0x4e, 0x8a, 0xaa, 0x2f, 0x1a, 0x95, 0x6b, 0xdc, 0x49 });
        /* {f2fcd37d-894a-4e71-8aaa-2f1a956bdc49} */

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        protected TokenizerBase(ILogManager log, StackedFileReader input) {

            if (log == null)
                ExceptionHelper.ArgumentIsNull(nameof(log));

            if (input == null)
                ExceptionHelper.ArgumentIsNull(nameof(input));

            Input = input;
            Log = new LogSource(log, TokenizerLogMessage);
            state = new TokenizerState(this, input, Log);
        }

        public bool AtEof
            => Input.CurrentFile == null;

        /// <summary>
        ///     interalstate
        /// </summary>
        private readonly TokenizerState state;

        /// <summary>
        ///     fetch the next token
        /// </summary>
        public void FetchNextToken()
            => currentToken = CharacterClasses.FetchNextToken(state);

        public char NextChar()
            => Input.NextChar();

        public char PreviousChar()
            => Input.PreviousChar();

        /// <summary>
        ///     keep whitspace literals
        /// </summary>
        public bool KeepWhitspace { get; set; }
            = false;

        /// <summary>
        ///     check if a token value should be kept
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool KeepTokenValue(int tokenId) {
            if (tokenId == TokenKind.WhiteSpace)
                return KeepWhitspace;
            return true;
        }

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        public Token CurrentToken
            => currentToken;

        private Token currentToken
            = new Token();

        /// <summary>
        ///     used char classes
        /// </summary>
        protected abstract InputPatterns CharacterClasses { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; }

        /// <summary>
        ///     inut
        /// </summary>
        public StackedFileReader Input { get; private set; }

    }
}