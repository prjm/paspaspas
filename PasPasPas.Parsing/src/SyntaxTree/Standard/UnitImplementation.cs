using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit implementation part
    /// </summary>
    public class UnitImplementation : StandardSyntaxTreeBase {

        /// <summary>
        ///     declaration section
        /// </summary>
        public Declarations DeclarationSections { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}