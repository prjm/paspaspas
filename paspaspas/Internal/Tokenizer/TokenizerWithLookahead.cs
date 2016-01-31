using PasPasPas.Api;
using System.Collections.Generic;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     tokenizer with lookahear
    /// </summary>
    public class TokenizerWithLookahead : IPascalTokenizer {

        /// <summary>
        ///     base tokenizer
        /// </summary>
        public IPascalTokenizer BaseTokenizer { get; set; }

        /// <summary>
        ///     list of tokens
        /// </summary>
        private List<PascalToken> tokenList = new List<PascalToken>();

        /// <summary>
        ///     check if other tokens are availiable
        /// </summary>
        /// <returns><c>false</c> if <c>eof</c> is reached</returns>
        public bool HasNextToken()
            => (tokenList.Count > 0) || BaseTokenizer.HasNextToken();

        private void InternalFetchNextToken() {
            if (!HasNextToken()) {
                tokenList.Add(new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty });
                return;
            }

            int currentTokenCount = tokenList.Count;
            while (tokenList.Count == currentTokenCount) {
                var nextToken = BaseTokenizer.FetchNextToken();
                if (IsValidToken(nextToken))
                    tokenList.Add(nextToken);
            }
        }

        private static bool IsValidToken(PascalToken nextToken)
            => nextToken.Kind != PascalToken.WhiteSpace;

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
                return new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };
            }
            else {
                return tokenList[num];
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public PascalToken FetchNextToken() {
            PascalToken result;
            if (tokenList.Count > 0) {
                result = tokenList[0];
                tokenList.RemoveAt(0);
            }
            else {
                result = BaseTokenizer.FetchNextToken();
            }
            return result;
        }
    }
}
