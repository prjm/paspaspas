using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     reintroduce directive
    /// </summary>
    public class ReintroduceDirective : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ReintroduceDirective(IParserInformationProvider informationProvider) : base(informationProvider) { }


        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("reintroduce").Punct(";");
        }
    }
}
