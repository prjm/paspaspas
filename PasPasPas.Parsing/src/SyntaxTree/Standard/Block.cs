using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     file block
    /// </summary>
    public class Block : StandardSyntaxTreeBase {

        /// <summary>
        ///     block body
        /// </summary>
        public BlockBody Body { get; set; }

        /// <summary>
        ///     declarations
        /// </summary>
        public Declarations DeclarationSections { get; set; }

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