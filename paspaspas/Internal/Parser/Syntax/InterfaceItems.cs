using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     items of an interface declaration
    /// </summary>
    public class InterfaceItems : ComposedPart<InterfaceItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public InterfaceItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format interface declration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
            result.NewLine();
        }
    }
}