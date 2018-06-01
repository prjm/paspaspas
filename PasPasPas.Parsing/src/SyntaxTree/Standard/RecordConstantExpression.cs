using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class for a record constant expressopn
    /// </summary>
    public class RecordConstantExpression : StandardSyntaxTreeBase {


        /// <summary>
        ///     field name
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     field value
        /// </summary>
        public ConstantExpressionSymbol Value { get; set; }

        /// <summary>
        ///     separator
        /// </summary>
        public Terminal Separator { get; set; }

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