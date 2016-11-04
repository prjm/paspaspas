using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     comparison expression
    /// </summary>
    public class ComparisonExpression : ExpressionBase {

        /// <summary>
        ///     expression kind
        /// </summary>
        public ComparisonExpressionKind Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public ExpressionOperand LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public ExpressionOperand RightOperand { get; set; }

        /// <summary>
        ///     convert an expression kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static ComparisonExpressionKind ConvertKind(int kind) {

            switch (kind) {
                case TokenKind.LessThen:
                    return ComparisonExpressionKind.LessThen;

                case TokenKind.LessThenEquals:
                    return ComparisonExpressionKind.LessThenEquals;

                case TokenKind.GreaterThen:
                    return ComparisonExpressionKind.GreaterThen;

                case TokenKind.GreaterThenEquals:
                    return ComparisonExpressionKind.GreaterThenEquals;

                case TokenKind.NotEquals:
                    return ComparisonExpressionKind.NotEquals;

                case TokenKind.EqualsSign:
                    return ComparisonExpressionKind.EqualsSign;

                case TokenKind.In:
                    return ComparisonExpressionKind.In;

                case TokenKind.As:
                    return ComparisonExpressionKind.As;

                default:
                    return ComparisonExpressionKind.Undefined;
            }

        }

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (LeftOperand != null)
                    yield return LeftOperand;
                if (RightOperand != null)
                    yield return RightOperand;
            }
        }
    }
}
