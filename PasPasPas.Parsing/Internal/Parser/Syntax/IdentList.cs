using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     comma separated list of identifiers
    /// </summary>
    public class IdentList : ComposedPart<PascalIdentifier> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public IdentList(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format identifierts
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(", "));
        }
    }
}