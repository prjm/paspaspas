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
                if (invalidTokensBefore.IsValueCreated) {
                    foreach (Token token in invalidTokensBefore.Value)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     invalid tokens after this token
        /// </summary>
        public IEnumerable<Token> InvalidTokensAfter {
            get {
                if (invalidTokensAfter.IsValueCreated) {
                    foreach (Token token in invalidTokensAfter.Value)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     list of invalid tokens before this token
        /// </summary>
        private Lazy<IList<Token>> invalidTokensBefore =
            new Lazy<IList<Token>>(() => new List<Token>());

        /// <summary>
        ///     list of invalid tokens after this token
        /// </summary>
        private Lazy<IList<Token>> invalidTokensAfter =
            new Lazy<IList<Token>>(() => new List<Token>());

        /// <summary>
        ///     assign remaining tokens
        /// </summary>
        /// <param name="invalidTokens"></param>
        /// <param name="afterwards">add tokens after this token</param>
        public void AssignInvalidTokens(Queue<Token> invalidTokens, bool afterwards) {
            if (invalidTokens.Count > 0) {
                foreach (Token token in invalidTokens)
                    if (afterwards)
                        invalidTokensAfter.Value.Add(token);
                    else
                        invalidTokensBefore.Value.Add(token);

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

