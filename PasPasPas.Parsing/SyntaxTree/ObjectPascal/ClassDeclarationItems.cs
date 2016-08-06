using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class declaration items
    /// </summary>
    public class ClassDeclarationItems : ComposedPart<ClassDeclarationItem> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassDeclarationItems(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format class declarations
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.NewLine());
        }
    }
}