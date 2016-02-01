using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingDirective : SyntaxPartBase {

        /// <summary>
        ///     create a new directive
        /// </summary>
        /// <param name="informationProvider"></param>
        public BindingDirective(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     message expression
        /// </summary>
        public ExpressionBase MessageExpression { get; internal set; }

        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Message:
                    result.Keyword("message").Space();
                    result.Part(MessageExpression);
                    break;
                case PascalToken.Static:
                    result.Keyword("static");
                    break;
                case PascalToken.Dynamic:
                    result.Keyword("dynamic");
                    break;
                case PascalToken.Virtual:
                    result.Keyword("virtual");
                    break;
                case PascalToken.Override:
                    result.Keyword("override");
                    break;
            }
            result.Punct(";");
        }
    }
}
