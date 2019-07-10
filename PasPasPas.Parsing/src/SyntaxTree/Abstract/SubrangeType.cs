using System;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     subrange type
    /// </summary>
    public class SubrangeType : TypeSpecificationBase, IExpressionTarget {

        /// <summary>
        ///     expression kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public IExpression RangeStart { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public IExpression RangeEnd { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => RangeEnd ?? RangeStart;

            set {
                if (RangeStart == null)
                    RangeStart = value;
                else if (RangeEnd == null)
                    RangeEnd = value;
                else
                    throw new InvalidOperationException();
            }

        }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, RangeStart, visitor);
            AcceptPart(this, RangeEnd, visitor);
            visitor.EndVisit(this);
        }

    }
}
