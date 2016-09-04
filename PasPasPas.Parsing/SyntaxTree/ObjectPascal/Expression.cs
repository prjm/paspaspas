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
                    case TokenKind.LessThen:
                        result.Operator("<");
                        break;
                    case TokenKind.LessThenEquals:
                        result.Operator("<=");
                        break;
                    case TokenKind.GreaterThen:
                        result.Operator(">");
                        break;
                    case TokenKind.GreaterThenEquals:
                        result.Operator(">=");
                        break;
                    case TokenKind.NotEquals:
                        result.Operator("<>");
                        break;
                    case TokenKind.EqualsSign:
                        result.Operator("=");
                        break;
                    case TokenKind.In:
                        result.Operator("in");
                        break;
                    case TokenKind.Is:
                        result.Operator("is");
                        break;
                }
                result.Space();
                result.Part(RightOperand);
            }
        }
    }
}
