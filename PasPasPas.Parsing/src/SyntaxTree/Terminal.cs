using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using static PasPasPas.Parsing.Tokenizer.TokenizerWithLookahead;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     syntax tree terminal
    /// </summary>
    public class Terminal : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new terminal token
        /// </summary>
        /// <param name="baseToken"></param>
        public Terminal(TokenSequence baseToken) {
            if (baseToken == null) {
                Token = Token.Empty;
                Prefix = string.Empty;
                Suffix = string.Empty;
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
        public string Suffix { get; }

        /// <summary>
        ///     prefix
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => (Prefix ?? string.Empty).Length +
               (Suffix ?? string.Empty).Length +
               (Token.Value ?? string.Empty).Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
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
        public static int GetSymbolKind(this Terminal terminal) => terminal == null ? TokenKind.Undefined : terminal.Kind;

    }

}