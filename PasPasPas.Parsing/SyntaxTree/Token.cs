using PasPasPas.Infrastructure.Input;
using System.Globalization;
using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     token definition
    /// </summary>
    public struct Token {

        private static Token empty
            = new Token();

        private static Token eof
                    = new Token(TokenKind.Eof, 0, string.Empty);

        /// <summary>
        ///     empty token
        /// </summary>
        public static ref Token Empty
            => ref empty;

        /// <summary>
        ///     empty token
        /// </summary>
        public static ref Token Eof
            => ref eof;


        /// <summary>
        ///     Token value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Token kind
        /// </summary>
        public int Kind { get; }

        /// <summary>
        ///     token position
        /// </summary>
        public int Position { get; }

        /// <summary>
        ///     invalid tokens before this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensBefore {
            get {
                if (invalidTokensBefore != null) {
                    foreach (var token in invalidTokensBefore)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     invalid tokens after this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensAfter {
            get {
                if (invalidTokensAfter != null) {
                    foreach (var token in invalidTokensAfter)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     list of invalid tokens before this token
        /// </summary>
        private IList<Token> invalidTokensBefore;

        /// <summary>
        ///     list of invalid tokens after this token
        /// </summary>
        private IList<Token> invalidTokensAfter;

        /// <summary>
        ///     create a new syntax token
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="tokenPosition">token position</param>
        /// <param name="value">token value</param>
        public Token(int tokenKind, int tokenPosition, string value) : this() {
            Kind = tokenKind;
            Position = tokenPosition;
            Value = value.Pool();
        }

        /// <summary>
        ///     create a new syntax token
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="tokenPosition">token position</param>
        /// <param name="value">token value</param>
        public Token(int tokenKind, int tokenPosition, char value) : this() {
            Kind = tokenKind;
            Position = tokenPosition;
            Value = value.Pool();
        }


        /// <summary>
        ///     assign remaining tokens
        /// </summary>
        /// <param name="invalidTokens"></param>
        /// <param name="afterwards">add tokens after this token</param>
        public void AssignInvalidTokens(IndexedQueue<Token> invalidTokens, bool afterwards) {

            if (invalidTokens.Count > 0) {

                if (afterwards && invalidTokensAfter == null)
                    invalidTokensAfter = new List<Token>(invalidTokens.Count);
                else if (invalidTokensBefore == null)
                    invalidTokensBefore = new List<Token>(invalidTokens.Count);


                foreach (var token in invalidTokens)
                    if (afterwards)
                        invalidTokensAfter.Add(token);
                    else
                        invalidTokensBefore.Add(token);

                invalidTokens.Clear();
            }
        }

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => Kind.ToString(CultureInfo.InvariantCulture) + ": " + Value?.Trim();

    }

}

