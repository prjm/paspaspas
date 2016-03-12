using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     generic defnition part
    /// </summary>
    public class GenericDefinitionPart : ComposedPart<ConstrainedGeneric> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public GenericDefinitionPart(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     parse identifiert
        /// </summary>
        public PascalIdentifier Identifier { get; internal set; }

        /// <summary>
        ///     format generic definition part
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Identifier(Identifier.Value);

            if (Count > 0) {
                result.Space();
                result.Punct(":");
                result.Space();

                FlattenToPascal(result, x => x.Punct(", "));
            }
        }
    }
}