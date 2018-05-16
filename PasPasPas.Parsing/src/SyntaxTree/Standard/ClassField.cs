using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple field declaration
    /// </summary>
    public class ClassField : StandardSyntaxTreeBase {
        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     names
        /// </summary>
        public IdentifierList Names { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; set; }


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