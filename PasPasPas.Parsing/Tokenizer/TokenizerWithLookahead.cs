using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a tokenizer with a lookead list
    /// </summary>
    public abstract class TokenizerWithLookahead {

        /// <summary>
        ///     protected constructor
        /// </summary>
        protected TokenizerWithLookahead(ITokenizer baseTokenizer) =>
            BaseTokenizer = baseTokenizer;

        /// <summary>
        ///     base tokenizer
        /// </summary>
        public ITokenizer BaseTokenizer { get; private set; }

        /// <summary>
        ///     list of tokens
        /// </summary>
        private IndexedQueue<Token> tokenList
            = new IndexedQueue<Token>();

        /// <summary>
        ///     list of invalid tokens (e.g. whitespace)
        /// </summary>
        private IndexedQueue<Token> invalidTokens
            = new IndexedQueue<Token>();

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
                    if (tokenList.Count > 0)
                        tokenList.Last.AssignInvalidTokens(invalidTokens, true);
                    return;
                }

                BaseTokenizer.FetchNextToken();
                var nextToken = BaseTokenizer.CurrentToken;

                if (IsValidToken(nextToken)) {
                    nextToken.AssignInvalidTokens(invalidTokens, false);
                    tokenList.Enqueue(nextToken);
                }
                else {
                    if (IsMacroToken(nextToken))
                        ProcessMacroToken(nextToken);
                    invalidTokens.Enqueue(nextToken);
                }
            }
        }

        /// <summary>
        ///     process a macro token
        /// </summary>
        /// <param name="nextToken">token to process</param>
        protected abstract void ProcessMacroToken(Token nextToken);

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken">token to test</param>
        /// <returns><c>true</c> if the token is a macro token</returns>
        protected abstract bool IsMacroToken(Token nextToken);

        /// <summary>
        ///     test if a token is relevant and valid
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected abstract bool IsValidToken(Token nextToken);

        /// <summary>
        ///     gets the current token
        /// </summary>
        public Token CurrentToken
            => LookAhead(0);

        public bool AtEof => throw new NotImplementedException();

        public char CurrentCharacter => throw new NotImplementedException();

        public int CurrentPosition => throw new NotImplementedException();

        public ILogSource Log => throw new NotImplementedException();

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
                return tokenList[number];
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

        public char NextChar() => throw new NotImplementedException();
        public char PreviousChar() => throw new NotImplementedException();
        public bool PrepareNextToken() => throw new NotImplementedException();
    }
}
