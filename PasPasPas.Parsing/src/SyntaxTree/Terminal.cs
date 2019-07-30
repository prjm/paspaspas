using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : StandardSyntaxTreeBase, IEquatable<Terminal> {

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="baseToken"></param>
        public Terminal(TokenSequence baseToken) {
            if (baseToken == null) {
                Token = Token.Empty;
                Prefix = default;
                Suffix = default;
            }
            else {
                Token = baseToken.Value;
                Prefix = baseToken.Prefix;
                Suffix = baseToken.Suffix;
            }
        }

        /// <summary>
        ///     token
        /// </summary>
        public Token Token { get; }

        /// <summary>
        ///     token value
        /// </summary>
        public string Value
            => Token.Value;

        /// <summary>
        ///     token kind
        /// </summary>
        public int Kind
            => Token.Kind;

        /// <summary>
        ///     suffix
        /// </summary>
        public ImmutableArray<Token> Suffix { get; }

        /// <summary>
        ///     prefix
        /// </summary>
        public ImmutableArray<Token> Prefix { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length {
            get {
                var result = 0;

                if (Prefix != null)
                    foreach (var token in Prefix)
                        result += (token.Value ?? string.Empty).Length;

                if (Suffix != null)
                    foreach (var token in Suffix)
                        result += (token.Value ?? string.Empty).Length;

                return result + (Token.Value ?? string.Empty).Length;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Terminal other) {

            if (!Token.Equals(other.Token))
                return false;

            if (other.Prefix == null && Prefix != null || other.Prefix != null && Prefix == null)
                return false;

            if (other.Suffix == null && Suffix != null || other.Suffix != null && Suffix == null)
                return false;

            if (Prefix != null) {
                if (other.Prefix.Length != Prefix.Length)
                    return false;
                for (var i = 0; i < Prefix.Length; i++)
                    if (!Prefix[i].Equals(other.Prefix[i]))
                        return false;
            }

            if (Suffix != null) {
                if (other.Suffix.Length != Suffix.Length)
                    return false;
                for (var i = 0; i < Suffix.Length; i++)
                    if (!Suffix[i].Equals(other.Suffix[i]))
                        return false;

            }



            return true;
        }

        /// <summary>
        ///     cinoare fir equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is Terminal terminal)
                return Equals(terminal);
            return false;
        }

        /// <summary>
        ///     compute a hash code for this terminal
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                result = result * 31 + Token.GetHashCode();

                if (Prefix != default)
                    for (var i = 0; i < Prefix.Length; i++)
                        result = result * 31 + Prefix[i].GetHashCode();

                if (Suffix != default)
                    for (var i = 0; i < Suffix.Length; i++)
                        result = result * 31 + Suffix[i].GetHashCode();

                return result;
            }
        }
    }

    /// <summary>
    ///     helper for terminals
    /// </summary>
    public static class TerminalHelper {

        /// <summary>
        ///     get the terminal kind
        /// </summary>
        /// <param name="terminal"></param>
        /// <returns></returns>
        public static int GetSymbolKind(this Terminal terminal)
            => terminal == null ? TokenKind.Undefined : terminal.Kind;

    }

}