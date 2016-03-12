using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     list of namespace names
    /// </summary>
    public class NamespaceNameList : ComposedPart<NamespaceName> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public NamespaceNameList(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format namespace name list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(",").NewLine());
            result.Punct(";").NewLine().NewLine();
        }
    }
}