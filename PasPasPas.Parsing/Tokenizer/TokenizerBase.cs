using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase : ITokenizer {

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
        }

        public void PrepareNextToken() {
            var file = Input.CurrentFile;

            while (file != null && file.AtEof)
                file = Input.FinishCurrentFile();
        }

        public virtual bool AtEof {
            get {
                var file = Input.CurrentFile;
                return file == null || file.AtEof;
            }
        }

        /// <summary>
        ///     fetch the next token
        /// </summary>
        public virtual void FetchNextToken()
            => CurrentToken = CharacterClasses.FetchNextToken(this);

        public void NextChar()
            => Input.NextChar();

        public void PreviousChar()
            => Input.PreviousChar();

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        public Token CurrentToken { get; private set; }

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

        /// <summary>
        ///     get the current value
        /// </summary>
        public char CurrentCharacter
            => Input.Value;

        /// <summary>
        ///     get the current position
        /// </summary>
        public int CurrentPosition
            => Input.Position;
    }
}