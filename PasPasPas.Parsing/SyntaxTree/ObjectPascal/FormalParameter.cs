using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameter : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public FormalParameter(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     default value
        /// </summary>
        public Expression DefaultValue { get; internal set; }

        /// <summary>
        ///     parse a list of identifiers
        /// </summary>
        public IdentList ParamNames { get; internal set; }

        /// <summary>
        ///     parameter typs
        /// </summary>
        public int ParamType { get; internal set; }
            = PascalToken.Undefined;

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; internal set; }

        /// <summary>
        ///     format paremeter
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Attributes.Count > 0) {
                Attributes.ToFormatter(result);
                result.Space();
            }

            if (ParamType != PascalToken.Undefined) {
                if (ParamType == PascalToken.Const) {
                    result.Keyword("const");
                }
                else if (ParamType == PascalToken.Var) {
                    result.Keyword("var");
                }
                else if (ParamType == PascalToken.Out) {
                    result.Keyword("out");
                }
                result.Space();
            }

            ParamNames.ToFormatter(result);

            if (TypeDeclaration != null) {
                result.Space();
                result.Punct(":");
                result.Space();
                TypeDeclaration.ToFormatter(result);
            }

            if (DefaultValue != null) {
                result.Space();
                result.Punct(" = ");
                result.Space();
                DefaultValue.ToFormatter(result);
            }
        }
    }
}