using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm statement
    /// </summary>
    public class AsmStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     opcode
        /// </summary>
        public AsmOpCode OpCode { get; set; }

        /// <summary>
        ///     lock / segment prefix
        /// </summary>
        public AsmPrefix Prefix { get; set; }

        /// <summary>
        ///     label
        /// </summary>
        public AsmLabel Label { get; set; }

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
