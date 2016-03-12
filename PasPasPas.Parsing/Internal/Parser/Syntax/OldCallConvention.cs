using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     call convention
    /// </summary>
    public class OldCallConvention : SyntaxPartBase {

        /// <summary>
        ///     create new call convention
        /// </summary>
        /// <param name="informationProvider"></param>
        public OldCallConvention(IParserInformationProvider informationProvider) : base(informationProvider) {

        }

        /// <summary>
        ///     call convention kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format call convention
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Near:
                    result.Keyword("near");
                    break;

                case PascalToken.Far:
                    result.Keyword("far");
                    break;

                case PascalToken.Local:
                    result.Keyword("local");
                    break;
            }
            result.Punct(";");
        }
    }
}
