using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration
    /// </summary>
    public class MethodDeclaration : StandardSyntaxTreeBase {
        public MethodDeclaration(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

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

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool Class { get; set; }

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectives Directives { get; set; }

        /// <summary>
        ///     method heading
        /// </summary>
        public MethodDeclarationHeading Heading { get; set; }

        /// <summary>
        ///     method implementation
        /// </summary>
        public Block MethodBody { get; set; }

    }
}
