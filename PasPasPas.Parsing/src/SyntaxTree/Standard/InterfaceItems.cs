using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     items of an interface declaration
    /// </summary>
    public class InterfaceItems : VariableLengthSyntaxTreeBase<InterfaceItem> {

        /// <summary>
        ///     create a new list of interface items
        /// </summary>
        /// <param name="items"></param>
        public InterfaceItems(ImmutableArray<InterfaceItem> items) : base(items) {
        }

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