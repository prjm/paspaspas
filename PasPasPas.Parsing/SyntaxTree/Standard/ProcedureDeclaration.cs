using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration
    /// </summary>
    public class ProcedureDeclaration : StandardSyntaxTreeBase {

        public ProcedureDeclaration(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; set; }

        /// <summary>
        ///     procedure declaration heading
        /// </summary>
        public ProcedureDeclarationHeading Heading { get; set; }

        /// <summary>
        ///     procedure body
        /// </summary>
        public Block ProcedureBody { get; set; }

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
