using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     object items
    /// </summary>
    public class ObjectItems : ComposedPart<SyntaxPartBase> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ObjectItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format items
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
            result.NewLine();
        }
    }
}