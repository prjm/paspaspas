using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedurual type
    /// </summary>
    public class ProcedureType : StandardSyntaxTreeBase {

        /// <summary>
        ///     procedure reference
        /// </summary>
        public ProcedureReference ProcedureReference { get; set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureRefType { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}