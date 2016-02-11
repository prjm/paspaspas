using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     repeat statement
    /// </summary>
    public class RepeatStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new repeat statement
        /// </summary>
        /// <param name="parser"></param>
        public RepeatStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; internal set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; internal set; }

        /// <summary>
        ///     format repeat statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("repeat");
            result.StartIndent();
            result.NewLine();
            result.Part(Statements);
            result.EndIndent();
            result.NewLine();
            result.Keyword("until").Space();
            result.Part(Condition);
            result.NewLine();
        }
    }
}