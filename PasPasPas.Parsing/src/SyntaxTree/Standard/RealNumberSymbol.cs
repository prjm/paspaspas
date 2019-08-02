using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     real literal
    /// </summary>
    public class RealNumberSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new real number
        /// </summary>
        /// <param name="symbol"></param>
        public RealNumberSymbol(Terminal symbol)
            => Symbol = symbol;

        /// <summary>
        ///     terminal
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     length
        /// </summary>
        public override int Length
            => Symbol.GetSymbolLength();


    }
}
