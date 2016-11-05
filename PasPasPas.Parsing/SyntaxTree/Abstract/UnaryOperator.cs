
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     unary operator
    /// </summary>
    public class UnaryOperator : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     operator kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     operator parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (Value != null)
                    yield return Value;
            }
        }

    }
}
