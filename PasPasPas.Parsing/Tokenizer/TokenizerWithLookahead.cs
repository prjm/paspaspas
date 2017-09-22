using System;
using System.Collections.Generic;
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
    public sealed class TokenizerWithLookahead : ITokenizer {

        /// <summary>
        ///     a sequence of fetched tokens
        /// </summary>
        private class TokenSequence {

            private Lazy<LinkedList<Token>> prefix
                = new Lazy<LinkedList<Token>>(false);

            private Lazy<LinkedList<Token>> suffix
                = new Lazy<LinkedList<Token>>(false);

            public Token Value = default;

            public void AssignPrefix(IEnumerable<Token> tokens) {
                var p = prefix.Value;
                foreach (var token in tokens)
                    p.AddLast(token);
            }

            public void AssignSuffix(IEnumerable<Token> tokens) {
                var p = suffix.Value;
                foreach (var token in tokens)
                    p.AddLast(token);
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
        private IndexedQueue<TokenSequence> tokenList
            = new IndexedQueue<TokenSequence>();

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
                        tokenList.Last.AssignSuffix(invalidTokens);
                        invalidTokens.Clear();
                    }
                    return;
                }

                BaseTokenizer.FetchNextToken();
                var nextToken = BaseTokenizer.CurrentToken;

                if (IsValidToken(ref nextToken)) {
                    var entry = new TokenSequence {
                        Value = nextToken
                    };
                    entry.AssignPrefix(invalidTokens);
                    invalidTokens.Clear();
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
                return tokenList[number].Value;
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public void FetchNextToken() {
            var result = LookAhead(0);
            if (tokenList.Count > 0) {
                tokenList.Dequeue();
            }
        }

        /// <summary>
        ///     dispose the basic tokenizer
        /// </summary>
        public void Dispose() {
            BaseTokenizer.Dispose();
            BaseTokenizer = null;
        }
    }
}
