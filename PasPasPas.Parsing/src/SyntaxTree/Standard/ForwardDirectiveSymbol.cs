using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     forward directive
    /// </summary>
    public class ForwardDirectiveSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new forward directive symbol
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="semicolon"></param>
        public ForwardDirectiveSymbol(Terminal directive, Terminal semicolon) {
            Directive = directive;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

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