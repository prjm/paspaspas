using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     method directrives
    /// </summary>
    public class MethodDirectives : ComposedPart<SyntaxPartBase> {


        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodDirectives(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format directives
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Space());
        }
    }
}