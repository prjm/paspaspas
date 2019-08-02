using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     Quoted string
    /// </summary>
    public class QuotedStringSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     quoted string
        /// </summary>
        /// <param name="stringSymbol"></param>
        public QuotedStringSymbol(Terminal stringSymbol)
            => Symbol = stringSymbol;

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }


        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Symbol.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            visitor.EndVisit(this);
        }


    }
}