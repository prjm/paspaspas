using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable value
    /// </summary>
    public class VarValueSpecification : StandardSyntaxTreeBase {

        /// <summary>
        ///     absolute index
        /// </summary>
        public ConstantExpression Absolute { get; set; }

        /// <summary>
        ///     initial value
        /// </summary>
        public ConstantExpression InitialValue { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }



    }
}