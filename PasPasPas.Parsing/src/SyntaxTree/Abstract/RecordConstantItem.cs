using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     record constant item
    /// </summary>
    public class RecordConstantItem : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     constant item value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }
    }
}
