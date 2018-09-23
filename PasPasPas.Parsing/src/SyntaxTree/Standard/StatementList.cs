using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     statement list
    /// </summary>
    public class StatementList : VariableLengthSyntaxTreeBase<StatementSymbol> {

        /// <summary>
        ///     create a new statement list
        /// </summary>
        /// <param name="items"></param>
        public StatementList(ImmutableArray<StatementSymbol> items) : base(items) {
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

