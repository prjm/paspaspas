using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetSectn : ComposedPart<SetSectnPart> {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="informationProvider"></param>
        public SetSectn(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     format set section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("[");
            FlattenToPascal(result, null);
            result.Punct("]");
        }
    }
}