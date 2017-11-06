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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}