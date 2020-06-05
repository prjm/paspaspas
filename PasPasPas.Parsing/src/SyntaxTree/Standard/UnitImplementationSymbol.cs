#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit implementation part
    /// </summary>
    public class UnitImplementationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unit implementation symbol
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="usesClause"></param>
        /// <param name="declarationSections"></param>
        public UnitImplementationSymbol(Terminal implementation, UsesClauseSymbol usesClause, DeclarationsSymbol declarationSections) {
            Implementation = implementation;
            UsesClause = usesClause;
            DeclarationSections = declarationSections;
        }

        /// <summary>
        ///     declaration section
        /// </summary>
        public DeclarationsSymbol DeclarationSections { get; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClauseSymbol UsesClause { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Implementation.GetSymbolLength() +
                UsesClause.GetSymbolLength() +
                DeclarationSections.GetSymbolLength();

        /// <summary>
        ///     implementation symbol
        /// </summary>
        public Terminal Implementation { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Implementation, visitor);
            AcceptPart(this, UsesClause, visitor);
            AcceptPart(this, DeclarationSections, visitor);
            visitor.EndVisit(this);
        }


    }
}