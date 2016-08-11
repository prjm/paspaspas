using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     simple expression
    /// </summary>
    public class SimplExpr : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public SimplExpr(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     expression kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Term LeftOperand { get; internal set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimplExpr RightOperand { get; internal set; }

        /// <summary>
        ///     format expression
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(LeftOperand);
            if (RightOperand != null) {
                result.Space();
                switch (Kind) {
                    case TokenKind.Plus:
                        result.Punct("+");
                        break;

                    case TokenKind.Minus:
                        result.Punct("-");
                        break;

                    case PascalToken.Or:
                        result.Keyword("or");
                        break;

                    case PascalToken.Xor:
                        result.Keyword("xor");
                        break;

                }
                result.Space();
                result.Part(RightOperand);
            }
        }
    }
}