using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     hint list
    /// </summary>
    public class HintingInformationList : VariableLengthSyntaxTreeBase<HintSymbol> {

        /// <summary>
        ///     create a new hinting information list
        /// </summary>
        /// <param name="items"></param>
        public HintingInformationList(ImmutableArray<HintSymbol> items) : base(items) {

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
        public override int Length => ItemLength;

    }
}
