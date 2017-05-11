using PasPasPas.Infrastructure.Input;
using System.Globalization;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     token definition
    /// </summary>
    public class Token {

        /// <summary>
        ///     create a new token
        /// </summary>
        public Token() {
            Kind = TokenKind.Undefined;
            Value = string.Empty;
            FilePath = null;
        }

        /// <summary>
        ///     Token value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Token kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     token start position
        /// </summary>
        public TextFilePosition StartPosition { get; internal set; }

        /// <summary>
        ///     token end position
        /// </summary>
        public TextFilePosition EndPosition { get; internal set; }

        /// <summary>
        ///     invalid tokens before this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensBefore {
            get {
                if (invalidTokensBefore != null) {
                    foreach (Token token in invalidTokensBefore)
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
                    foreach (Token token in invalidTokensAfter)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     list of invalid tokens before this token
        /// </summary>
        private IList<Token> invalidTokensBefore = null;

        /// <summary>
        ///     list of invalid tokens after this token
        /// </summary>
        private IList<Token> invalidTokensAfter = null;

        /// <summary>
        ///     assign remaining tokens
        /// </summary>
        /// <param name="invalidTokens"></param>
        /// <param name="afterwards">add tokens after this token</param>
        public void AssignInvalidTokens(Queue<Token> invalidTokens, bool afterwards) {

            if (invalidTokens.Count > 0) {

                if (afterwards && invalidTokensAfter == null)
                    invalidTokensAfter = new List<Token>(invalidTokens.Count);
                else if (invalidTokensBefore == null)
                    invalidTokensBefore = new List<Token>(invalidTokens.Count);


                foreach (Token token in invalidTokens)
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

    /// <summary>
    ///     string literal token
    /// </summary>
    public class StringLiteralToken : Token {

        /// <summary>
        ///     string value
        /// </summary>
        public string LiteralValue { get; set; }

    }

    /// <summary>
    ///     string literal token
    /// </summary>
    public class IntegerLiteralToken : Token {

        /// <summary>
        ///     int value
        /// </summary>
        public int LiteralValue { get; set; }

    }

    /// <summary>
    ///     number token
    /// </summary>
    public class NumberLiteralToken : Token {


        /// <summary>
        ///     int value
        /// </summary>
        public int LiteralValue { get; set; }

    }

}

