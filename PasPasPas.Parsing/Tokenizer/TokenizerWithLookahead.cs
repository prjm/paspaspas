using System;
using System.Collections.Generic;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer mode
    /// </summary>
    public enum TokenizerMode {

        /// <summary>
        ///     undefined tokenizer mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     standard tokenizer
        /// </summary>
        Standard = 1,

        /// <summary>
        ///     compiler directive tokenizer
        /// </summary>
        CompilerDirective = 2
    };

    /// <summary>
    ///     base class for a tokenizer with a lookead list
    /// </summary>
    public sealed class TokenizerWithLookahead : ITokenizer, IDisposable {

        private static Guid tokenSequencePool
            = new Guid(new byte[] { 0x9b, 0xd7, 0xb5, 0x3a, 0xc2, 0xf6, 0x6a, 0x47, 0xb4, 0x29, 0x4a, 0xd8, 0x19, 0x73, 0x8e, 0x56 });
        /* {3ab5d79b-f6c2-476a-b429-4ad819738e56} */

        /// <summary>
        ///     a sequence of fetched tokens
        /// </summary>
        public class TokenSequence : IPoolItem {

            private string prefix
                = string.Empty;

            private string suffix
                = string.Empty;

            /// <summary>
            ///     token value
            /// </summary>
            public Token Value { get; set; }

            /// <summary>
            ///     token prefix
            /// </summary>
            public string Prefix
                => prefix;

            /// <summary>
            ///     token suffix (invalid to parser)
            /// </summary>
            public string Suffix
                => suffix;

            /// <summary>
            ///     gets the buffer current prefix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignPrefix(Queue<Token> tokens, IParserEnvironment environment) {
                using (var poolItem = environment.StringBuilderPool.Borrow()) {
                    var sb = poolItem.Data;
                    while (tokens.Count > 0)
                        sb.Append(tokens.Dequeue().Value);
                    prefix = sb.ToString();
                }
            }

            /// <summary>
            ///     get the current suffix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignSuffix(Queue<Token> tokens, IParserEnvironment environment) {
                using (var poolItem = environment.StringBuilderPool.Borrow()) {
                    var sb = poolItem.Data;
                    while (tokens.Count > 0)
                        sb.Append(tokens.Dequeue().Value);
                    suffix = sb.ToString();
                }
            }

            /// <summary>
            ///     clear the tokenizer
            /// </summary>
            public void Clear() {
                prefix = null;
                suffix = null;
                Value = default;
            }

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            /// <param name="tokenKind"></param>
            /// <returns></returns>
            public bool MatchesKind(int[] tokenKind) {

                for (var i = 0; i < tokenKind.Length; i++) {
                    if (tokenKind[i] == Value.Kind)
                        return true;
                }

                return false;
            }
        }

        private OptionSet options;
        private TokenizerMode mode = TokenizerMode.Undefined;
        private readonly IParserEnvironment environment;
        private readonly IRuntimeValues constValues;

        /// <summary>
        ///     create a new tokenizer with lookahead
        /// </summary>
        public TokenizerWithLookahead(IParserEnvironment env, OptionSet optionsSet, ITokenizer baseTokenizer, TokenizerMode tokenizerMode) {
            mode = tokenizerMode;
            BaseTokenizer = baseTokenizer;
            options = optionsSet;
            environment = env;
            constValues = env.ConstantValues;
        }

        /// <summary>
        ///     base tokenizer
        /// </summary>
        public ITokenizer BaseTokenizer { get; private set; }

        /// <summary>
        ///     list of tokens
        /// </summary>
        private IndexedQueue<ObjectPool<TokenSequence>.PoolItem> tokenList
            = new IndexedQueue<ObjectPool<TokenSequence>.PoolItem>();

        /// <summary>
        ///     list of invalid tokens (e.g. whitespace)
        /// </summary>
        private Queue<Token> invalidTokens
            = new Queue<Token>();

        /// <summary>
        ///     check if a next token exists
        /// </summary>
        public bool HasNextToken
            => (tokenList.Count > 0) || !BaseTokenizer.AtEof;

        /// <summary>
        ///     fetch a next token / fill token list
        /// </summary>
        private void InternalFetchNextToken() {
            var currentTokenCount = tokenList.Count;

            while (tokenList.Count == currentTokenCount || tokenList.Count < 2) {

                if (BaseTokenizer.AtEof) {
                    if (tokenList.Count > 0) {
                        var item = tokenList.Last.Data;
                        item.AssignSuffix(invalidTokens, environment);
                    }
                    return;
                }

                var currentInput = BaseTokenizer.Input.CurrentFile.File;
                BaseTokenizer.FetchNextToken();
                var nextToken = BaseTokenizer.CurrentToken;

                if (IsValidToken(ref nextToken)) {
                    var entry = environment.TokenSequencePool.Borrow();
                    entry.Data.Value = nextToken;
                    entry.Data.AssignPrefix(invalidTokens, environment);
                    tokenList.Enqueue(entry);
                }
                else {
                    if (IsMacroToken(ref nextToken))
                        ProcessMacroToken(currentInput, ref nextToken);
                    invalidTokens.Enqueue(nextToken);
                }
            }
        }

        /// <summary>
        ///     check if a given token is a valid token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        private bool IsValidToken(ref Token nextToken) {

            switch (mode) {

                case TokenizerMode.Standard:
                    return (nextToken.Kind != TokenKind.WhiteSpace) &&
                        nextToken.Kind != TokenKind.ControlChar &&
                        nextToken.Kind != TokenKind.Comment &&
                        nextToken.Kind != TokenKind.Preprocessor &&
                        (!options.ConditionalCompilation.Skip);

                case TokenizerMode.CompilerDirective:
                    return nextToken.Kind != TokenKind.WhiteSpace &&
                        nextToken.Kind != TokenKind.ControlChar;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken">token to test</param>
        /// <returns><c>true</c> if the token is a macro token</returns>
        private bool IsMacroToken(ref Token nextToken) {
            if (mode == TokenizerMode.Standard)
                return nextToken.Kind == TokenKind.Preprocessor;
            return false;
        }

        /// <summary>
        ///     do nothing
        /// </summary>
        /// <param name="nextToken"></param>
        /// <param name="path">current path</param>
        private void ProcessMacroToken(IFileReference path, ref Token nextToken) {
            var patterns = environment.Patterns.CompilerDirectivePatterns;
            var fragmentBuffer = new FileBuffer();
            var reader = new StackedFileReader(fragmentBuffer);
            var macroValue = nextToken.ParsedValue as IStringValue;
            fragmentBuffer.Add(path, new StringBufferReadable(macroValue.AsUnicodeString));
            reader.AddFileToRead(path);

            using (var parser = new CompilerDirectiveParser(environment, options, reader)) {
                var result = parser.Parse();
                var visitor = new CompilerDirectiveVisitor(options, path, Log);
                result.Accept(visitor.AsVisitor());
            }
        }

        /// <summary>
        ///     gets the current token
        /// </summary>
        public Token CurrentToken
            => LookAhead(0).Value;

        /// <summary>
        ///     gets the current token
        /// </summary>
        public TokenSequence CurrentTokenSequence
            => LookAhead(0);

        /// <summary>
        ///     check if <c>eof</c> is reached
        /// </summary>
        public bool AtEof
            => tokenList.Count < 1 && (BaseTokenizer == null || BaseTokenizer.AtEof);

        /// <summary>
        ///     registered keywords
        /// </summary>
        public IDictionary<string, int> Keywords =>
            BaseTokenizer.Keywords;

        /// <summary>
        ///     file input
        /// </summary>
        public StackedFileReader Input
            => BaseTokenizer.Input;

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log
            => BaseTokenizer.Log;

        /// <summary>
        ///     get tokens and look ahader
        /// </summary>
        /// <param name="number">number of tokens to look ahead</param>
        /// <returns>token</returns>
        public TokenSequence LookAhead(int number) {
            checked {
                while (!BaseTokenizer.AtEof && (tokenList.Count < Math.Max(2, 1 + number))) {
                    InternalFetchNextToken();
                }
            }

            if (tokenList.Count <= number) {
                return new TokenSequence() {
                    Value = Token.Empty
                };
            }
            else {
                return tokenList[number].Data;
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public void FetchNextToken() {
            var result = LookAhead(0);
            if (tokenList.Count > 0) {
                var data = tokenList.Dequeue();
                data.Dispose();
                data = null;
            }
        }

        /// <summary>
        ///     dispose the basic tokenizer
        /// </summary>
        public void Dispose() {
            while (tokenList != null && tokenList.Count > 0) {
                var data = tokenList.Dequeue();
                data.Dispose();
                data = null;
            }

            if (BaseTokenizer != null) {
                BaseTokenizer.Dispose();
                BaseTokenizer = null;
            }
        }
    }
}
