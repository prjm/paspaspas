using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a constant value
    /// </summary>
    public class ConstantValue : ExpressionBase {

        /// <summary>
        ///     constant value kind
        /// </summary>
        public ConstantValueKind Kind { get; set; }

        /// <summary>
        ///     integer value
        /// </summary>
        public int IntValue { get; internal set; }

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
