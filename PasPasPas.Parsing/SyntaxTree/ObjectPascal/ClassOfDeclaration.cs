using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class of declaration
    /// </summary>
    public class ClassOfDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassOfDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     type name
        /// </summary>
        public NamespaceName TypeName { get; internal set; }

        /// <summary>
        ///     format class of type definition
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("class");
            result.Space();
            result.Keyword("of");
            result.Space();
            TypeName.ToFormatter(result);
        }
    }
}