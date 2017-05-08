using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type declaration
    /// </summary>
    public class TypeDeclaration : StandardSyntaxTreeBase {
        public TypeDeclaration(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     type speicifcaiton
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public GenericTypeIdentifier TypeId { get; set; }


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