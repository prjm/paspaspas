using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///    method declaration names
    /// </summary>
    public class MethodDeclarationName : StandardSyntaxTreeBase {

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     generic parameters
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }

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
