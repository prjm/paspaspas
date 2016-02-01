using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     hint list
    /// </summary>
    public class HintingInformationList : ComposedPart<HintingInformation> {

        /// <summary>
        ///     create new hint list
        /// </summary>
        /// <param name="informationProvider"></param>
        public HintingInformationList(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format hint list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Space());
        }
    }
}
