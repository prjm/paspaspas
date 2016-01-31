using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     enumeration type definition
    /// </summary>
    public class EnumTypeDefinition : ComposedPart<EnumValue> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public EnumTypeDefinition(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format enum type definition
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("(");
            FlattenToPascal(result, x => x.Punct(","));
            result.Punct(")");
        }
    }
}