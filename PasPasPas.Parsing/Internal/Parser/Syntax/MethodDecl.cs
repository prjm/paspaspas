using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     method declaration
    /// </summary>
    public class MethodDecl : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodDecl(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool Class { get; internal set; }

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectives Directives { get; internal set; }

        /// <summary>
        ///     method heading
        /// </summary>
        public MethodDeclHeading Heading { get; internal set; }

        /// <summary>
        ///     method implementation
        /// </summary>
        public Block MethodBody { get; internal set; }


        /// <summary>
        ///     format method declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Attributes);
            if (Class)
                result.Keyword("class").Space();
            result.Part(Heading).Space();
            result.Punct(";").NewLine();
            result.Part(Directives).NewLine();
            if ((MethodBody != null) && (MethodBody.Body != null)) {
                result.Part(MethodBody);
                result.Punct(";").NewLine();
            }
        }
    }
}
