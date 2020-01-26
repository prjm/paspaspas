using System;
using System.Globalization;
using PasPasPas.Globals.Runtime;

#if DESKTOP
using PasPasPas.Desktop.BackwardCompatibility;
#endif

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     basic structure for a syntax token
    /// </summary>
    public readonly struct Token : IEquatable<Token> {

        /// <summary>
        ///     empty token
        /// </summary>
        public static readonly Token Empty
            = new Token(TokenKind.Empty, default);

        /// <summary>
        ///     Token value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Token kind
        /// </summary>
        public int Kind { get; }

        /// <summary>
        ///     parsed token value (if any)
        /// </summary>
        public IValue ParsedValue { get; }

        /// <summary>
        ///     token length
        /// </summary>
        public int Length
            => Value.Length;

        /// <summary>
        ///     create a new token
        /// </summary>
        /// <param name="tokenId">token id</param>
        /// <param name="value">value</param>
        /// <param name="parsedValue">parser literal value (optional)</param>
        public Token(int tokenId, string value, IValue parsedValue = null) : this() {
            Kind = tokenId;
            Value = value ?? string.Empty;
            ParsedValue = parsedValue;
        }

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "{0}: {1}", Kind, Value);

        /// <summary>
        ///     compare to another token
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Token other)
            => Kind == other.Kind && string.Equals(Value, other.Value, StringComparison.Ordinal);

        /// <summary>
        ///     compare tokens
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is Token token)
                return Equals(token);
            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            unchecked {
                return 17 + 23 * Kind + 11 * Value.GetHashCode(StringComparison.Ordinal);
            }
        }

        /// <summary>
        ///     compare two tokens
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Token left, Token right)
            => left.Equals(right);

        /// <summary>
        ///     compare two tokens
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Token left, Token right)
            => !(left == right);

    }

}

