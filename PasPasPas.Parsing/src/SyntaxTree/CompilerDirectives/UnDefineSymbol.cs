using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     compiler directive to undefine a symbol
    /// </summary>
    public class UnDefineSymbol : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal conditional;

        /// <summary>
        ///     undefine directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="conditional"></param>
        public UnDefineSymbol(Terminal symbol, Terminal conditional) {
            this.symbol = symbol;
            this.conditional = conditional;
        }

        /// <summary>
        ///     undefined symbol name
        /// </summary>
        public string SymbolName
            => conditional.Value;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, conditional, visitor);
            visitor.EndVisit(this);
        }

    }
}
