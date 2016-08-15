using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     expression
    /// </summary>
    public class Expression : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider"></param>
        public Expression(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     closue expression
        /// </summary>
        public ClosureExpr ClosureExpression { get; internal set; }

        /// <summary>
        ///     relational operator kind
        /// </summary>
        public int Kind { get; internal set; } = TokenKind.Undefined;

        /// <summary>
        ///     simple expression
        /// </summary>
        public SimplExpr LeftOperand { get; internal set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimplExpr RightOperand { get; internal set; }

        /// <summary>
        ///     format expression
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(ClosureExpression);
            result.Part(LeftOperand);
            if (RightOperand != null) {
                result.Space();
                switch (Kind) {
                    case PascalToken.LessThen:
                        result.Operator("<");
                        break;
                    case PascalToken.LessThenEquals:
                        result.Operator("<=");
                        break;
                    case PascalToken.GreaterThen:
                        result.Operator(">");
                        break;
                    case PascalToken.GreaterThenEquals:
                        result.Operator(">=");
                        break;
                    case PascalToken.NotEquals:
                        result.Operator("<>");
                        break;
                    case TokenKind.EqualsSign:
                        result.Operator("=");
                        break;
                    case TokenKind.In:
                        result.Operator("in");
                        break;
                    case PascalToken.Is:
                        result.Operator("is");
                        break;
                }
                result.Space();
                result.Part(RightOperand);
            }
        }
    }
}
