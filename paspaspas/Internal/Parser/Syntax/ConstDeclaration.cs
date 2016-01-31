using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     constant declaration
    /// </summary>
    public class ConstDeclaration : DeclarationBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ConstDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     user defined attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     additional hint for that constant
        /// </summary>
        public HintingInformation Hint { get; internal set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public PascalIdentifier Identifier { get; internal set; }

        /// <summary>
        ///     type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; internal set; }

        /// <summary>
        ///     expression
        /// </summary>
        public ConstantExpression Value { get; internal set; }

        /// <summary>
        ///     format identifier
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            if (Attributes.Count > 0) {
                Attributes.ToFormatter(result);
                result.NewLine();
            }

            result.Identifier(Identifier.Value);
            result.Space();

            if (TypeSpecification != null) {
                result.Punct(":");
                result.Space();
                TypeSpecification.ToFormatter(result);
                result.Space();
            }

            result.Operator("=");
            result.Space();
            Value.ToFormatter(result);
            Hint.ToFormatter(result);
            result.Punct(";");
            result.NewLine();
        }
    }
}
