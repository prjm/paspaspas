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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
