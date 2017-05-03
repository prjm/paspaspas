using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm prefix
    /// </summary>
    public class AsmPrefix : StandardSyntaxTreeBase {

        /// <summary>
        ///     lock prefix
        /// </summary>
        public Identifier LockPrefix { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public Identifier SegmentPrefix { get; set; }

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
