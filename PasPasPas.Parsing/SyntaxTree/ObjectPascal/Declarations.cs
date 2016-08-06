using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     common declarations
    /// </summary>
    public class Declarations : ComposedPart<SyntaxPartBase> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Declarations(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format declarations
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, _ => { });
        }


    }
}