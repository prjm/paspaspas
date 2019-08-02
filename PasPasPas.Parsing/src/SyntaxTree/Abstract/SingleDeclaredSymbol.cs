using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     single declared symbol
    /// </summary>
    public class SingleDeclaredSymbol : DeclaredSymbolGroup {

        /// <summary>
        ///     create a new symbol group
        /// </summary>
        /// <param name="baseSymbol">symbol to be wrapped</param>
        public SingleDeclaredSymbol(AbstractSyntaxPartBase baseSymbol)
            => Symbol = baseSymbol;

        /// <summary>
        ///     visit this symbol group
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol
        /// </summary>
        public AbstractSyntaxPartBase Symbol { get; set; }
    }
}
