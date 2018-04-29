using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
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
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (RangeStart != null)
                    yield return RangeStart;
                if (RangeEnd != null)
                    yield return RangeEnd;
            }
        }

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
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
