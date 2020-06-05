#nullable disable
using System;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     binary operator
    /// </summary>
    public class BinaryOperator : ExpressionBase, IExpressionTarget, IRequiresArrayExpression {

        /// <summary>
        ///     expression kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public IExpression LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public IExpression RightOperand { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => RightOperand ?? LeftOperand;

            set {
                if (LeftOperand == null)
                    LeftOperand = value;
                else if (RightOperand == null)
                    RightOperand = value;
                else
                    throw new InvalidProgramException();
            }
        }

        /// <summary>
        ///     <c>true</c> if an array operator is required
        /// </summary>
        public bool RequiresArray { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, RightOperand, visitor);
            visitor.EndVisit(this);
        }

    }
}
