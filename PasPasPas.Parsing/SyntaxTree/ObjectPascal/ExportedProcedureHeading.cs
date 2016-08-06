using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     exported procedure heading for an interace section
    /// </summary>
    public class ExportedProcedureHeading : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExportedProcedureHeading(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectives Directives { get; internal set; }

        /// <summary>
        ///     heading kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     exported proc name
        /// </summary>
        public PascalIdentifier Name { get; internal set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; internal set; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; internal set; }

        /// <summary>
        ///     result types
        /// </summary>
        public TypeSpecification ResultType { get; internal set; }

        /// <summary>
        ///     format exported procedure headiing
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Kind == PascalToken.Function) {
                result.Keyword("function");
            }
            if (Kind == PascalToken.Procedure) {
                result.Keyword("procedure");
            }
            result.Space();
            result.Part(Name);
            result.Part(Parameters);
            if (ResultType != null) {
                result.Punct(":").Space();
                result.Part(ResultAttributes);
                result.Part(ResultType);
            }
            result.Punct(";");
        }
    }
}
