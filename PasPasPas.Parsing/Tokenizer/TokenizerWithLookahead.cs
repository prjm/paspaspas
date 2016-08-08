using PasPasPas.Api;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer with lookahear
    /// </summary>
    public abstract class TokenizerWithLookahead : ITokenizer {

        /// <summary>
        ///     protected constructo
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
        private Queue<PascalToken> tokenList = new Queue<PascalToken>();

        /// <summary>
        ///     list of invalid tokens (e.g. whitespace)
        /// </summary>
        private Queue<PascalToken> invalidTokens = new Queue<PascalToken>();


        /// <summary>
        ///     check if other tokens are availiable
        /// </summary>
        /// <returns><c>false</c> if <c>eof</c> is reached</returns>
        public bool HasNextToken()
            => (tokenList.Count > 0) || BaseTokenizer.HasNextToken();

        private void InternalFetchNextToken() {
            int currentTokenCount = tokenList.Count;
            while (tokenList.Count == currentTokenCount) {

                if (!HasNextToken()) {
                    //tokenList.Enqueue(new PascalToken(PascalToken.Eof, string.Empty, new FileReference(string.Empty)));
                    return;
                }

                var nextToken = BaseTokenizer.FetchNextToken();
                if (IsValidToken(nextToken)) {
                    nextToken.AssignRemainingTokens(invalidTokens);
                    tokenList.Enqueue(nextToken);
                }
                else {
                    if (IsMacroToken(nextToken))
                        ProcssMacroToken(nextToken);
                    invalidTokens.Enqueue(nextToken);
                }
            }
        }

        /// <summary>
        ///     process preprocessor token
        /// </summary>
        /// <param name="nextToken"></param>
        protected abstract void ProcssMacroToken(PascalToken nextToken);

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected abstract bool IsMacroToken(PascalToken nextToken);

        /// <summary>
        ///     test if a token is relevant and valid
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected abstract bool IsValidToken(PascalToken nextToken);

        /// <summary>
        ///     gets the current token
        /// </summary>
        public PascalToken CurrentToken()
            => LookAhead(0);

        /// <summary>
        ///     get tokens and look ahader
        /// </summary>
        /// <param name="num">number of tokens to look ahead</param>
        /// <returns>token</returns>
        public PascalToken LookAhead(int num) {
            checked {
                while (BaseTokenizer.HasNextToken() && (tokenList.Count < 1 + num)) {
                    InternalFetchNextToken();
                }
            }

            if (tokenList.Count <= num) {
                return null; //  new PascalToken(PascalToken.Eof, string.Empty, new FileReference(string.Empty));
            }
            else {
                return tokenList.ElementAt(num);
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public PascalToken FetchNextToken() {
            PascalToken result = LookAhead(0);
            if (tokenList.Count > 0) {
                tokenList.Dequeue();
            }
            return result;
        }
    }
}
