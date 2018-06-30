using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a type specification
    /// </summary>
    public class TypeSpecification : StandardSyntaxTreeBase {

        /// <summary>
        ///     pointer type
        /// </summary>
        public PointerType PointerType { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureType ProcedureType { get; set; }

        /// <summary>
        ///     simple type
        /// </summary>
        public SimpleType SimpleType { get; set; }

        /// <summary>
        ///     string type
        /// </summary>
        public StringType StringType { get; set; }

        /// <summary>
        ///     structured type
        /// </summary>
        public StructType StructuredType { get; set; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; set; }

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