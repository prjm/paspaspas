using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration items
    /// </summary>
    public class ClassDeclarationItemsSymbol : VariableLengthSyntaxTreeBase<ClassDeclarationItemSymbol> {

        /// <summary>
        ///     create a new set of class declaration items
        /// </summary>
        /// <param name="items"></param>
        public ClassDeclarationItemsSymbol(ImmutableArray<ClassDeclarationItemSymbol> items) : base(items) {
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength;

    }
}