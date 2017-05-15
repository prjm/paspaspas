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
        public ConstantExpression Value { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}