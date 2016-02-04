using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     unsafe directive
    /// </summary>
    public class UnsafeDirective : SyntaxPartBase {

        /// <summary>
        ///     create new directive
        /// </summary>
        /// <param name="informationProvider"></param>
        public UnsafeDirective(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("unsafe").Punct(";");
        }
    }
}
