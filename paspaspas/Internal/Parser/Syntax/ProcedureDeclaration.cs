using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     procedure declaration
    /// </summary>
    public class ProcedureDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProcedureDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; internal set; }

        /// <summary>
        ///     procedure declaration heading
        /// </summary>
        public ProcedureDeclarationHeading Heading { get; internal set; }

        /// <summary>
        ///     procedure body
        /// </summary>
        public ProcBody ProcBody { get; internal set; }

        /// <summary>
        ///     format procedure declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Heading);
            result.Punct(";").NewLine();
            result.Part(Directives).NewLine();
            result.Part(ProcBody);
        }
    }
}
