using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     compiler directive to define a symbol
    /// </summary>
    public class DefineSymbol : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal name;

        /// <summary>
        ///     create a new directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="name"></param>
        public DefineSymbol(Terminal symbol, Terminal name) {
            this.symbol = symbol;
            this.name = name;
        }

        /// <summary>
        ///     defined symbol name
        /// </summary>
        public string SymbolName
            => name?.Value;

        /// <summary>
        ///     identifier
        /// </summary>
        public Terminal Identifier
            => name;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, name, visitor);
            visitor.EndVisit(this);
        }
    }
}
