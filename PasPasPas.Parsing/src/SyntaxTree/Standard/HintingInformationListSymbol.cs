using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     hint list
    /// </summary>
    public class HintingInformationListSymbol : VariableLengthSyntaxTreeBase<HintSymbol> {

        /// <summary>
        ///     create a new hinting information list
        /// </summary>
        /// <param name="items"></param>
        public HintingInformationListSymbol(ImmutableArray<HintSymbol> items) : base(items) {

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
