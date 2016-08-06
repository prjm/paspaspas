using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     overload directive
    /// </summary>
    public class OverloadDirective : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public OverloadDirective(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format overload directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("overload").Punct(";");
        }
    }
}
