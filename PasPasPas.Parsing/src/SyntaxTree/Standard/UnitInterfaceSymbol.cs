using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface part of a unit
    /// </summary>
    public class UnitInterfaceSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     interface declaration
        /// </summary>
        public InterfaceDeclaration InterfaceDeclaration { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public ISyntaxPart UsesClause { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => InterfaceSymbol.Length + UsesClause.Length + InterfaceDeclaration.Length;

        /// <summary>
        ///     interface symbol
        /// </summary>
        public Terminal InterfaceSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, InterfaceSymbol, visitor);
            AcceptPart(this, UsesClause, visitor);
            AcceptPart(this, InterfaceDeclaration, visitor);
            visitor.EndVisit(this);
        }


    }
}