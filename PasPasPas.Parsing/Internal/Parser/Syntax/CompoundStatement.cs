using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public CompoundStatement(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; internal set; }

        /// <summary>
        /// format compound statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("begin").StartIndent();
            result.NewLine();
            result.Part(Statements);
            result.Keyword("end").EndIndent();
            result.NewLine();
        }
    }
}