#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     call convention
    /// </summary>
    public class OldCallConventionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new old call convention symbol
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="semicolon"></param>
        public OldCallConventionSymbol(Terminal directive, Terminal semicolon) {
            Directive = directive;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     call convention kind
        /// </summary>
        public int Kind
            => Directive.GetSymbolKind();

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() + Semicolon.GetSymbolLength();
    }
}
