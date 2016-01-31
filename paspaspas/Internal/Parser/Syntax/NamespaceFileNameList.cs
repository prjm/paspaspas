using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     namespace file name list
    /// </summary>
    public class NamespaceFileNameList : ComposedPart<NamespaceFileName> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public NamespaceFileNameList(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format namespace file name list
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(",").NewLine());
            result.Punct(";");
        }
    }
}