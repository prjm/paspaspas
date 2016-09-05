using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingDirective : SyntaxPartBase {

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     message expression
        /// </summary>
        public Expression MessageExpression { get; internal set; }

        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case TokenKind.Message:
                    result.Keyword("message").Space();
                    result.Part(MessageExpression);
                    break;
                case TokenKind.Static:
                    result.Keyword("static");
                    break;
                case TokenKind.Dynamic:
                    result.Keyword("dynamic");
                    break;
                case TokenKind.Virtual:
                    result.Keyword("virtual");
                    break;
                case TokenKind.Override:
                    result.Keyword("override");
                    break;
            }
            result.Punct(";");
        }
    }
}
