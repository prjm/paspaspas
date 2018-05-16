using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     external symbol declaration
    /// </summary>
    public class ExternalSymbolDeclaration : CompilerDirective {

        /// <summary>
        ///     identifier
        /// </summary>
        public string IdentifierName { get; set; }

        /// <summary>
        ///     symbol
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        ///     union
        /// </summary>
        public string UnionName { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
