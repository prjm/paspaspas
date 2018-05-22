using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     file block
    /// </summary>
    public class BlockSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     block body
        /// </summary>
        public SyntaxPartBase Body { get; set; }

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
            AcceptPart(this, DeclarationSections, visitor);
            AcceptPart(this, Body, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => DeclarationSections.Length + Body.Length;

    }
}