using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     procedure reference
    /// </summary>
    public class ProcedureReference : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProcedureReference(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinition ProcedureType { get; internal set; }

        /// <summary>
        ///     format procedure reference
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("reference").Space().Keyword("to").Space();
            result.Part(ProcedureType);
        }
    }
}