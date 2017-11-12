using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration
    /// </summary>
    public class ProcedureDeclaration : StandardSyntaxTreeBase {

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
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
