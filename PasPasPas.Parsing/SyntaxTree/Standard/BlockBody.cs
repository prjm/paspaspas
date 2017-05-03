using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBody : StandardSyntaxTreeBase {

        /// <summary>
        ///    assembler block
        /// </summary>
        public AsmBlock AssemblerBlock { get; set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatement Body { get; set; }


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