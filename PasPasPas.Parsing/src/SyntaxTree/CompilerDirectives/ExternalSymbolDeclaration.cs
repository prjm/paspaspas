using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     external symbol declaration
    /// </summary>
    public class ExternalSymbolDeclaration : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal identifier;
        private readonly Terminal symbolName;
        private readonly Terminal unionName;

        public ExternalSymbolDeclaration(Terminal symbol, Terminal identifier, Terminal symbolName, Terminal unionName) {
            this.symbol = symbol;
            this.identifier = identifier;
            this.symbolName = symbolName;
            this.unionName = unionName;
        }

        /// <summary>
        ///     identifier
        /// </summary>
        public string IdentifierName
            => identifier?.Value;

        /// <summary>
        ///     symbol
        /// </summary>
        public string SymbolName
            => symbolName?.Value;

        /// <summary>
        ///     union
        /// </summary>
        public string UnionName
            => unionName?.Value;

        /// <summary>
        ///     identifier
        /// </summary>
        public Terminal Identifier
            => identifier;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, identifier, visitor);
            AcceptPart(this, symbolName, visitor);
            AcceptPart(this, unionName, visitor);
            visitor.EndVisit(this);
        }
    }
}
