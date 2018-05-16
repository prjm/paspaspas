using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     <c>true</c> if this is an array constant
        /// </summary>
        public bool IsArrayConstant { get; set; }

        /// <summary>
        ///     <c>true</c> if this in an record constant
        /// </summary>
        public bool IsRecordConstant { get; set; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public Expression Value { get; set; }

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