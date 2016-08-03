using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     procedure declaration heading
    /// </summary>
    public class ProcedureDeclarationHeading : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProcedureDeclarationHeading(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     procedure name
        /// </summary>
        public PascalIdentifier Name { get; internal set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; internal set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; internal set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; internal set; }

        /// <summary>
        ///     format procedure declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Function:
                    result.Keyword("function").Space();
                    break;
                case PascalToken.Procedure:
                    result.Keyword("procedure").Space();
                    break;
            }
            result.Part(Name).Space();
            result.Part(Parameters);
            if (ResultType != null) {
                result.Punct(":").Space();
                result.Part(ResultTypeAttributes).Space();
                result.Part(ResultType);
            }
        }
    }
}