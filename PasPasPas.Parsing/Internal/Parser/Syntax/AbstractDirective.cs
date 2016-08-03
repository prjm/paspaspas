using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     abstract directive
    /// </summary>
    public class AbstractDirective : SyntaxPartBase {

        /// <summary>
        ///     create new directive
        /// </summary>
        /// <param name="informationProvider"></param>
        public AbstractDirective(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     final or abstract
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     formt directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Final:
                    result.Keyword("final");
                    break;
                case PascalToken.Virtual:
                    result.Keyword("virtual");
                    break;
            }
            result.Punct(";");
        }
    }
}
