using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     inlining directive
    /// </summary>
    public class InlineSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new inline symbol
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="semicolon"></param>
        public InlineSymbol(Terminal directive, Terminal semicolon) {
            Directive = directive;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     inline or assembler
        /// </summary>
        public int Kind
            => Directive.Kind;

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() + Semicolon.GetSymbolLength();

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


    }
}
