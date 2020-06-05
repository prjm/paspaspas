#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     items of an interface declaration
    /// </summary>
    public class InterfaceItems : VariableLengthSyntaxTreeBase<InterfaceItemSymbol> {

        /// <summary>
        ///     create a new list of interface items
        /// </summary>
        /// <param name="items"></param>
        public InterfaceItems(ImmutableArray<InterfaceItemSymbol> items) : base(items) {
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