using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndex : StandardSyntaxTreeBase {

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpression StartIndex { get; set; }

        /// <summary>
        ///     end index
        /// </summary>
        public ConstantExpression EndIndex { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}