using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpression : StandardSyntaxTreeBase {

        public ConstantExpression(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     <c>true</c> if this is an array constant
        /// </summary>
        public bool IsSetConstant { get; set; }

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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}