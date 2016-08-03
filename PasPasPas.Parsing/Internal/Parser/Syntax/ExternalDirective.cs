using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     external directive
    /// </summary>
    public class ExternalDirective : ComposedPart<ExternalSpecifier> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExternalDirective(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression ExternalExpression { get; internal set; }

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format external directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.VarArgs:
                    result.Keyword("varargs");
                    break;
                case PascalToken.External:
                    result.Keyword("external");
                    break;
            }

            if (ExternalExpression != null) {
                result.Space().Part(ExternalExpression).Space();
                FlattenToPascal(result, x => x.Space());
            }

            result.Punct(";");
        }
    }
}
