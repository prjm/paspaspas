using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     comparison expression
    /// </summary>
    public class BinaryOperator : ExpressionBase, IExpressionTarget {

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
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (LeftOperand != null)
                    yield return LeftOperand;
                if (RightOperand != null)
                    yield return RightOperand;
            }
        }

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
