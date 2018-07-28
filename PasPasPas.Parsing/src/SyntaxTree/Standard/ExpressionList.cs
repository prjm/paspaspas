using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     a list of expressions
    /// </summary>
    public class ExpressionList : VariableLengthSyntaxTreeBase<Expression> {

        /// <summary>
        ///     create a new expression list
        /// </summary>
        /// <param name="items"></param>
        public ExpressionList(ImmutableArray<Expression> items) : base(items) {
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