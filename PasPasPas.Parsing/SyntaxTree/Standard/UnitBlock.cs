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
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}