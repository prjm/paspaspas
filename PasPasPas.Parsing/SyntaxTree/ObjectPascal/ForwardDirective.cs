using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     forward directive
    /// </summary>
    public class ForwardDirective : SyntaxPartBase {

        /// <summary>
        ///     create new directive
        /// </summary>
        /// <param name="informationProvider"></param>
        public ForwardDirective(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("forward").Punct(";");
        }
    }
}
