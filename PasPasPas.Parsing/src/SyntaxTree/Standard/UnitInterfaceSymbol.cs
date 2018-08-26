using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface part of a unit
    /// </summary>
    public class UnitInterfaceSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unit interface symbol
        /// </summary>
        /// <param name="interfaceSymbol"></param>
        /// <param name="usesClause"></param>
        /// <param name="interfaceDeclaration"></param>
        public UnitInterfaceSymbol(Terminal interfaceSymbol, UsesClauseSymbol usesClause, InterfaceDeclarationSymbol interfaceDeclaration) {
            InterfaceSymbol = interfaceSymbol;
            UsesClause = usesClause;
            InterfaceDeclaration = interfaceDeclaration;
        }

        /// <summary>
        ///     interface declaration
        /// </summary>
        public InterfaceDeclarationSymbol InterfaceDeclaration { get; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClauseSymbol UsesClause { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => InterfaceSymbol.GetSymbolLength() +
                UsesClause.GetSymbolLength() +
                InterfaceDeclaration.GetSymbolLength();

        /// <summary>
        ///     interface symbol
        /// </summary>
        public Terminal InterfaceSymbol { get; }

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