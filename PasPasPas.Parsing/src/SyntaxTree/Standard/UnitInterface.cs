using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface part of a unit
    /// </summary>
    public class UnitInterface : StandardSyntaxTreeBase {

        /// <summary>
        ///     interface declaration
        /// </summary>
        public InterfaceDeclaration InterfaceDeclaration { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}