using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclHeading : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodDeclHeading(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; internal set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     method name
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; internal set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; internal set; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; internal set; }

        /// <summary>
        ///     format headinger
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Constructor:
                    result.Keyword("constructor").Space();
                    break;
                case PascalToken.Destructor:
                    result.Keyword("destructor").Space();
                    break;
                case PascalToken.Function:
                    result.Keyword("function").Space();
                    break;
                case PascalToken.Procedure:
                    result.Keyword("procedure").Space();
                    break;
            }
            result.Part(Name).Space();
            result.Part(GenericDefinition).Space();
            result.Part(Parameters);
            if (ResultType != null) {
                result.Punct(":").Space();
                result.Part(ResultTypeAttributes).Space();
                result.Part(ResultType);
            }
        }
    }
}