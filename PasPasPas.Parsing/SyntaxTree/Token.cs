using System;
using PasPasPas.Infrastructure.Utils;
using System.Text;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Common;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     token definition
    /// </summary>
    public struct Token : IEquatable<Token> {

        /// <summary>
        ///     empty token
        /// </summary>
        public static readonly Token Empty
            = new Token();

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
        ///     parsed token value (if any)
        /// </summary>
        public IValue ParsedValue { get; }

        /// <summary>
        ///     create a new token
        /// </summary>
        /// <param name="tokenId">token id</param>
        /// <param name="state">tokenizer state</param>
        /// <param name="parsedValue">parser literal value (optional)</param>
        public Token(int tokenId, TokenizerState state, IValue parsedValue = null) : this() {
            Kind = tokenId;
            Position = state.StartPosition;
            Value = state.GetBufferContent();
            ParsedValue = parsedValue;
        }

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{Kind}: {Value} [{Position}]";

        /// <summary>
        ///     compare to another token
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Token other)
            => (Kind == other.Kind) && (string.Equals(Value, other.Value));
    }

}

