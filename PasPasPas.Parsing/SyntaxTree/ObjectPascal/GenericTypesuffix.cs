using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///    generic type siffox
    /// </summary>
    public class GenericTypesuffix : ComposedPart<TypeSpecification> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public GenericTypesuffix(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format type suffix
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("<");
            FlattenToPascal(result, x => x.Punct(","));
            result.Punct(">");
        }
    }
}