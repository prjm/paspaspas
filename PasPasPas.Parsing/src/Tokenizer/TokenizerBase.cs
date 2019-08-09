using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a tokenizer
    /// </summary>
    public sealed class TokenizerBase : ITokenizer, IDisposable {

        /// <summary>
        ///     message: unexpected token
        /// </summary>
        public const uint UnexpectedCharacter
            = 0x0390;

        /// <summary>
        ///     message: unexpected end of token
        /// </summary>
        /// <remarks>
        ///     data: expected-token-end sequence
        /// </remarks>
        public const uint UnexpectedEndOfToken
            = 0x0391;

        /// <summary>
        ///     message id: incomplete hex number
        /// </summary>
        public const uint IncompleteHexNumber
            = 0x0392;

        /// <summary>
        ///     message id: incomplete identifier
        /// </summary>
        public const uint IncompleteIdentifier
            = 0x0393;

        /// <summary>
        ///     message id: incomplete string
        /// </summary>
        public const uint IncompleteString
            = 0x0394;

        /// <summary>
        ///     registered input patterns
        /// </summary>
        private readonly InputPatterns characterClasses;

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        public TokenizerBase(IParserEnvironment environment, InputPatterns charClasses, IStackedFileReader input) {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Log = environment.Log.CreateLogSource(MessageGroups.Tokenizer);
            characterClasses = charClasses ?? throw new ArgumentNullException(nameof(charClasses));
            state = new TokenizerState(environment, this, input, Log);
            FinishInput();
        }

        /// <summary>
        ///     dispose the tokenizer
        /// </summary>
        public void Dispose() {
            state.Dispose();
            Input.Dispose();
        }

        /// <summary>
        ///     test if the end of the current file is reached
        /// </summary>
        public bool AtEof
            => Input.CurrentFile == null;

        /// <summary>
        ///     internal tokenizer state
        /// </summary>
        private readonly TokenizerState state;

        /// <summary>
        ///     fetch the next token
        /// </summary>
        public void FetchNextToken() {
            CurrentToken = characterClasses.FetchNextToken(state);
            Position += CurrentToken.Length;
            FinishInput();
        }

        /// <summary>
        ///     remove all read files from the input
        /// </summary>
        private void FinishInput() {
            var file = Input.CurrentFile;
            if (file != null && Input.AtEof) {
                while (file != null && Input.AtEof)
                    file = Input.FinishCurrentFile();
            }
        }

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        public Token CurrentToken { get; private set; }
            = Token.Empty;

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; }

        /// <summary>
        ///     tokenizer input
        /// </summary>
        public IStackedFileReader Input { get; }

        /// <summary>
        ///     registered keywords
        /// </summary>
        public IDictionary<string, int> Keywords
            => characterClasses.Keywords;

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager ITokenizer.Log =>
            Log.Manager;

        /// <summary>
        ///     current position
        /// </summary>
        public int Position { get; private set; }
    }
}