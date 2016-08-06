using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     function directive
    /// </summary>
    public class FunctionDirectives : ComposedPart<SyntaxPartBase> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public FunctionDirectives(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format directive
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Space());
        }
    }
}