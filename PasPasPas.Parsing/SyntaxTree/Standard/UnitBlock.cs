using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit block
    /// </summary>
    public class UnitBlock : StandardSyntaxTreeBase {

        /// <summary>
        ///     initializarion
        /// </summary>
        public UnitInitialization Initialization { get; set; }

        /// <summary>
        ///     main block
        /// </summary>
        public CompoundStatement MainBlock { get; set; }

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