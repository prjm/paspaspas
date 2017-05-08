using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exported procedure heading for an interace section
    /// </summary>
    public class ExportedProcedureHeading : StandardSyntaxTreeBase {
        public ExportedProcedureHeading(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; set; }

        /// <summary>
        ///     heading kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     exported proc name
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     result types
        /// </summary>
        public TypeSpecification ResultType { get; set; }

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
