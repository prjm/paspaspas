using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a tokenizer with a lookead list
    /// </summary>
    public abstract class TokenizerWithLookahead : ITokenizer {

        /// <summary>
        ///     protected constructor
        /// </summary>
        protected TokenizerWithLookahead() { }

        /// <summary>
        ///     base tokenizer
        /// </summary>
        public ITokenizer BaseTokenizer { get; set; }

        /// <summary>
        ///     parser input
        /// </summary>
        public StackedFileReader Input
            => BaseTokenizer.Input;

        /// <summary>
        ///     list of tokens
        /// </summary>
        private Queue<Token> tokenList
            = new Queue<Token>();

        /// <summary>
        ///     list of invalid tokens (e.g. whitespace)
        /// </summary>
        private Queue<Token> invalidTokens
            = new Queue<Token>();


        /// <summary>
        ///     check if other tokens are availiable
        /// </summary>
        /// <returns><c>false</c> if <c>eof</c> is reached</returns>
        public bool HasNextToken()
            => (tokenList.Count > 0) || BaseTokenizer.HasNextToken();

        private void InternalFetchNextToken() {
            int currentTokenCount = tokenList.Count;
            while (tokenList.Count == currentTokenCount || tokenList.Count < 2) {

                if (!BaseTokenizer.HasNextToken()) {
                    if (tokenList.Count > 0)
                        tokenList.Last().AssignInvalidTokens(invalidTokens, true);
                    //tokenList.Enqueue(new PascalToken(PascalToken.Eof, string.Empty, new FileReference(string.Empty)));
                    return;
                }

                var nextToken = BaseTokenizer.FetchNextToken();
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
        public Token CurrentToken()
            => LookAhead(0);

        /// <summary>
        ///     get tokens and look ahader
        /// </summary>
        /// <param name="number">number of tokens to look ahead</param>
        /// <returns>token</returns>
        public Token LookAhead(int number) {
            checked {
                while (BaseTokenizer.HasNextToken() && (tokenList.Count < Math.Max(2, 1 + number))) {
                    InternalFetchNextToken();
                }
            }

            if (tokenList.Count <= number) {
                return null; //  new PascalToken(PascalToken.Eof, string.Empty, new FileReference(string.Empty));
            }
            else {
                return tokenList.ElementAt(number);
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public Token FetchNextToken() {
            Token result = LookAhead(0);
            if (tokenList.Count > 0) {
                tokenList.Dequeue();
            }
            return result;
        }

        /// <summary>
        ///     skip until whitespace
        /// </summary>
        public void SkipUntilEol() {
            while (HasNextToken() && (CurrentToken().Kind != TokenKind.WhiteSpace || !LineCounter.ContainsNewLineChar(CurrentToken().Value))) {
                if (tokenList.Count > 0) {
                    var currentInvalidTokens = CurrentToken().InvalidTokensBefore;
                    invalidTokens.Enqueue(tokenList.Dequeue());
                }
                FetchNextToken();
            }
        }

    }
}
