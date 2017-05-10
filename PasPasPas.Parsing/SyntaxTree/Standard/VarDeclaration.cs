using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VarDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; set; }

        /// <summary>
        ///     var names
        /// </summary>
        public IdentifierList Identifiers { get; set; }

        /// <summary>
        ///     var types
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

        /// <summary>
        ///     var values
        /// </summary>
        public VarValueSpecification ValueSpecification { get; set; }


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