using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     a set definition
    /// </summary>
    public class SetDef : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public SetDef(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     inner type reference
        /// </summary>
        public TypeSpecification TypeDefinition { get; internal set; }

        /// <summary>
        ///     format set
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("set");
            result.Space();
            result.Keyword("of");
            result.Space();
            TypeDefinition.ToFormatter(result);
        }
    }
}