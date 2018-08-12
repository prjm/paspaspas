using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     additional hinting information
    /// </summary>
    public class HintSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new hint symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="semicolon"></param>
        public HintSymbol(Terminal symbol, Terminal semicolon) {
            Symbol = symbol;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     create a new hint symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="deprecatedComment"></param>
        /// <param name="semicolon"></param>
        public HintSymbol(Terminal symbol, QuotedString deprecatedComment, Terminal semicolon) {
            Symbol = symbol;
            DeprecatedComment = deprecatedComment;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     hint for deprecation
        /// </summary>
        public bool Deprecated
            => Symbol.GetSymbolKind() == TokenKind.Deprecated;

        /// <summary>
        ///     comment for deprecation
        /// </summary>
        public SyntaxPartBase DeprecatedComment { get; }

        /// <summary>
        ///     hint for experimental
        /// </summary>
        public bool Experimental
                        => Symbol.GetSymbolKind() == TokenKind.Experimental;

        /// <summary>
        ///     hint for library
        /// </summary>
        public bool Library
            => Symbol.GetSymbolKind() == TokenKind.Library;

        /// <summary>
        ///     hint for platform
        /// </summary>
        public bool Platform
            => Symbol.GetSymbolKind() == TokenKind.Platform;

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///    hint symbol
        /// </summary>
        public Terminal Symbol { get; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, DeprecatedComment, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Symbol.GetSymbolLength() +
               DeprecatedComment.GetSymbolLength() +
               Semicolon.GetSymbolLength();

    }
}