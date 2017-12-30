using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Tokenizer.Patterns;
using System.Collections.Generic;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public sealed class Tokenizer : ITokenizer, IDisposable {

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
        ///     message id: incomplete hex number
        /// </summary>
        public static readonly Guid IncompleteHexNumber
            = new Guid(new byte[] { 0xfc, 0x7b, 0x96, 0xb1, 0xaf, 0x9e, 0x7e, 0x4a, 0x88, 0x7c, 0xc9, 0xa9, 0xaa, 0xab, 0xa7, 0xa8 });
        /* {b1967bfc-9eaf-4a7e-887c-c9a9aaaba7a8} */

        /// <summary>
        ///     message id: incomplete identifier
        /// </summary>
        public static readonly Guid IncompleteIdentifier
            = new Guid(new byte[] { 0x68, 0x3f, 0x9c, 0x79, 0x2, 0xe0, 0xf3, 0x42, 0x98, 0x8a, 0xef, 0xa3, 0x59, 0x4f, 0xf9, 0x42 });
        /* {799c3f68-e002-42f3-988a-efa3594ff942} */

        /// <summary>
        ///     message id: incomplete string
        /// </summary>
        public static readonly Guid IncompleteString
            = new Guid(new byte[] { 0xc, 0x14, 0xd4, 0xaa, 0x13, 0x18, 0xcd, 0x4f, 0xa4, 0xae, 0x42, 0x7e, 0xc9, 0x2e, 0xd6, 0xab });
        /* {aad4140c-1813-4fcd-a4ae-427ec92ed6ab} */

        /// <summary>
        ///     registered input patterns
        /// </summary>
        private InputPatterns characterClasses;

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        public Tokenizer(IParserEnvironment environment, InputPatterns charClasses, StackedFileReader input) {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Log = new LogSource(environment.Log, TokenizerLogMessage);
            characterClasses = charClasses ?? throw new ArgumentNullException(nameof(charClasses));
            state = new TokenizerState(environment, this, input, Log);
            FinishInput();
        }

        /// <summary>
        ///     dispose the tokenizer
        /// </summary>
        public void Dispose()
            => state.Dispose();

        /// <summary>
        ///     test if the end of the current file is reached
        /// </summary>
        public bool AtEof
            => Input.CurrentFile == null;

        /// <summary>
        ///     interalstate
        /// </summary>
        private readonly TokenizerState state;

        /// <summary>
        ///     fetch the next token
        /// </summary>
        public void FetchNextToken() {
            currentToken = characterClasses.FetchNextToken(state);
            FinishInput();
        }

        /// <summary>
        ///     remove all read files from the input
        /// </summary>
        private void FinishInput() {
            var file = Input.CurrentFile;
            if (file != null && file.AtEof) {
                while (file != null && file.AtEof)
                    file = Input.FinishCurrentFile();
            }
        }

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        public Token CurrentToken
            => currentToken;

        private Token currentToken
            = Token.Empty;

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; }

        /// <summary>
        ///     tokenizer input
        /// </summary>
        public StackedFileReader Input { get; }

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
    }
}