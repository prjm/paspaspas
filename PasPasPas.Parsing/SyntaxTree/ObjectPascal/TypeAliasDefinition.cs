using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     type alias definition
    /// </summary>
    public class TypeAliasDefinition : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public TypeAliasDefinition(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     generic type suffix
        /// </summary>
        public GenericTypesuffix GenericSuffix { get; internal set; }

        /// <summary>
        ///     source type name
        /// </summary>
        public NamespaceName TypeName { get; internal set; }

        /// <summary>
        ///     format as pascal
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("type");
            result.Space();
            TypeName.ToFormatter(result);

            if (GenericSuffix != null) {
                GenericSuffix.ToFormatter(result);
            }
        }
    }
}