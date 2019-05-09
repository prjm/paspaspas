﻿using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a tokenizer
    /// </summary>
    public sealed class TokenizerBase : ITokenizer, IDisposable {

        /// <summary>
        ///     message: unexpected token
        /// </summary>
        public static readonly Guid UnexpectedCharacter
            = new Guid(new byte[] { 0xd0, 0x79, 0xa5, 0xd0, 0x34, 0xa6, 0xba, 0x4c, 0x9d, 0x6, 0xc, 0x69, 0xde, 0xa6, 0x9e, 0xb });
        /* {d0a579d0-a634-4cba-9d06-0c69dea69e0b} */

        /// <summary>
        ///     message: unexpected end of token
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
        private readonly InputPatterns characterClasses;

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        public TokenizerBase(IParserEnvironment environment, InputPatterns charClasses, StackedFileReader input) {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Log = environment.Log.CreateLogSource(MessageGroups.Tokenizer);
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

        /// <summary>
        ///     current position
        /// </summary>
        public int Position { get; private set; }
    }
}