#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     hex number literal
    /// </summary>
    public class HexNumberSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hex number
        /// </summary>
        /// <param name="symbol"></param>
        public HexNumberSymbol(Terminal symbol)
            => Symbol = symbol;

        /// <summary>
        ///     symbol
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
