using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     external specifier
    /// </summary>
    public class ExternalSpecifier : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExternalSpecifier(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression Expression { get; internal set; }

        /// <summary>
        ///     external specifier kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format specifier
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Name:
                    result.Keyword("name");
                    break;

                case PascalToken.Index:
                    result.Keyword("index");
                    break;
            }
            result.Space().Part(Expression);
        }
    }
}