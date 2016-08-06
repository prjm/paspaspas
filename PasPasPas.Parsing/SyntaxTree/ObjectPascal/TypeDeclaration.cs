using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     type declaration
    /// </summary>
    public class TypeDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public TypeDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationList Hint { get; internal set; }

        /// <summary>
        ///     type speicifcaiton
        /// </summary>
        public TypeSpecification TypeSpecification { get; internal set; }

        /// <summary>
        ///     type id
        /// </summary>
        public GenericTypeIdent TypeId { get; internal set; }

        /// <summary>
        ///     format type declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Attributes.Count > 0) {
                Attributes.ToFormatter(result);
                result.NewLine();
            }
            TypeId.ToFormatter(result);
            result.Space();
            result.Operator("=");
            result.Space();
            TypeSpecification.ToFormatter(result);
            Hint.ToFormatter(result);
            result.Punct(";");
        }
    }
}