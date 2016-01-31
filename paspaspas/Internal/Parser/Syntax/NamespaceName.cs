using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     namespaced name
    /// </summary>
    public class NamespaceName : ComposedPart<PascalIdentifier> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public NamespaceName(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     formats the namespace name
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct("."));
        }
    }

}