using System;
using System.Collections.Generic;
using System.Text;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer mode
    /// </summary>
    public enum TokenizerMode {
        Undefined = 0,
        Standard = 1,
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
        ///     generic pool
        /// </summary>
        public static readonly Guid TokenSequencePool = tokenSequencePool;

        /// <summary>
        ///     a sequence of fetched tokens
        /// </summary>
        public class TokenSequence : IPoolItem {

            private string prefix = null;
            private string suffix = null;
            public Token Value = default;

            public void AssignPrefix(Queue<Token> tokens) {
                using (var sb = PoolFactory.FetchStringBuilder()) {
                    while (tokens.Count > 0)
                        sb.Data.Append(tokens.Dequeue().Value);
                    prefix = sb.ToString();
                }
            }

            public void AssignSuffix(Queue<Token> tokens) {
                using (var sb = PoolFactory.FetchStringBuilder()) {
                    while (tokens.Count > 0)
                        sb.Data.Append(tokens.Dequeue().Value);
                    suffix = sb.ToString();
                }
            }

            public void Clear() {
                prefix = null;
                suffix = null;
                Value = default;
            }
        }

        private TokenizerMode mode = TokenizerMode.Undefined;
        private bool skip = false;

        /// <summary>
        ///     create a new tokenizer with lookahead
        /// </summary>
        public TokenizerWithLookahead(ITokenizer baseTokenizer, TokenizerMode tokenizerMode) {
            mode = tokenizerMode;
            BaseTokenizer = baseTokenizer;
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
                        item.AssignSuffix(invalidTokens);
                    }
                    return;
                }

                BaseTokenizer.FetchNextToken();
                var nextToken = BaseTokenizer.CurrentToken;

                if (IsValidToken(ref nextToken)) {
                    var entry = PoolFactory.FetchGenericItem<TokenSequence>(StaticDependency.TokenSequencePool);
                    entry.Data.Value = nextToken;
                    entry.Data.AssignPrefix(invalidTokens);
                    tokenList.Enqueue(entry);
                }
                else {
                    if (IsMacroToken(ref nextToken))
                        ProcessMacroToken(nextToken);
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
                        (!skip);

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
        private void ProcessMacroToken(Token nextToken) {
            /*
            using (var input = new StringInput(CompilerDirectiveTokenizer.Unwrap(nextToken.Value), nextToken.FilePath))
            using (var reader = new OldStackedFileReader()) {
                var parser = new CompilerDirectiveParser(environment, reader);
                var tokenizer = new CompilerDirectiveTokenizer(environment, reader);
                reader.AddFile(input);
                parser.BaseTokenizer = tokenizer;
                ISyntaxPart result = parser.Parse();
                var visitor = new CompilerDirectiveVisitor() { Environment = environment };
                result.Accept(visitor.AsVisitor());
            }
            */
        }

        /// <summary>
        ///     gets the current token
        /// </summary>
        public Token CurrentToken
            => LookAhead(0);

        public bool AtEof
            => tokenList.Count < 1 && (BaseTokenizer == null || BaseTokenizer.AtEof);

        /// <summary>
        ///     get tokens and look ahader
        /// </summary>
        /// <param name="number">number of tokens to look ahead</param>
        /// <returns>token</returns>
        public Token LookAhead(int number) {
            checked {
                while (!BaseTokenizer.AtEof && (tokenList.Count < Math.Max(2, 1 + number))) {
                    InternalFetchNextToken();
                }
            }

            if (tokenList.Count <= number) {
                return Token.Empty;
            }
            else {
                return tokenList[number].Data.Value;
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
