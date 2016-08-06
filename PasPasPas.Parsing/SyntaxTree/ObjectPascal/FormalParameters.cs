using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     parameter list
    /// </summary>
    public class FormalParameters : ComposedPart<FormalParameter> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public FormalParameters(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format parameters
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct("; "));
        }
    }
}