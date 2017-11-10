using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration heading
    /// </summary>
    public class ProcedureDeclarationHeading : StandardSyntaxTreeBase {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     procedure name
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }

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