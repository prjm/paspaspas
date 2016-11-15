using System.Collections.Generic;

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
        public ExpressionBase RangeStart { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public ExpressionBase RangeEnd { get; set; }

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (RangeStart != null)
                    yield return RangeStart;
                if (RangeEnd != null)
                    yield return RangeEnd;
            }
        }

        /// <summary>
        ///     expression value
        /// </summary>
        public ExpressionBase Value
        {
            get
            {
                if (RangeEnd != null)
                    return RangeEnd;
                else
                    return RangeStart;
            }

            set
            {
                if (RangeStart == null)
                    RangeStart = value;
                else if (RangeEnd == null)
                    RangeEnd = value;
            }

        }
    }
}
