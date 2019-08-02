using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     end region directive
    /// </summary>
    public class EndRegion : CompilerDirectiveBase {

        /// <summary>
        ///     create a new end region directive
        /// </summary>
        /// <param name="symbol"></param>
        public EndRegion(Terminal symbol)
            => Symbol = symbol;

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            visitor.EndVisit(this);
        }

    }
}
