using System.Collections.Generic;

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
        public ExpressionBase LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public ExpressionBase RightOperand { get; set; }

        /// <summary>
        ///     convert an expression kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static ExpressionKind ConvertKind(int kind) {

            switch (kind) {
                case TokenKind.LessThen:
                    return ExpressionKind.LessThen;

                case TokenKind.LessThenEquals:
                    return ExpressionKind.LessThenEquals;

                case TokenKind.GreaterThen:
                    return ExpressionKind.GreaterThen;

                case TokenKind.GreaterThenEquals:
                    return ExpressionKind.GreaterThenEquals;

                case TokenKind.NotEquals:
                    return ExpressionKind.NotEquals;

                case TokenKind.EqualsSign:
                    return ExpressionKind.EqualsSign;

                case TokenKind.In:
                    return ExpressionKind.In;

                case TokenKind.As:
                    return ExpressionKind.As;

                case TokenKind.Plus:
                    return ExpressionKind.Plus;

                case TokenKind.Minus:
                    return ExpressionKind.Minus;

                case TokenKind.Or:
                    return ExpressionKind.Or;

                case TokenKind.Xor:
                    return ExpressionKind.Xor;

                case TokenKind.Div:
                    return ExpressionKind.Div;

                case TokenKind.Times:
                    return ExpressionKind.Times;

                case TokenKind.Slash:
                    return ExpressionKind.Slash;

                case TokenKind.Mod:
                    return ExpressionKind.Mod;

                case TokenKind.And:
                    return ExpressionKind.And;

                case TokenKind.Shl:
                    return ExpressionKind.Shl;

                case TokenKind.Shr:
                    return ExpressionKind.Shr;

                default:
                    return ExpressionKind.Undefined;
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

        /// <summary>
        ///     expression value
        /// </summary>
        public ExpressionBase Value
        {
            get
            {
                if (RightOperand != null)
                    return RightOperand;
                else
                    return LeftOperand;
            }

            set
            {
                if (LeftOperand == null)
                    LeftOperand = value;
                else if (RightOperand == null)
                    RightOperand = value;
            }
        }
    }
}
