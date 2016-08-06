using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///  while statement
    /// </summary>
    public class WhileStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new while statement
        /// </summary>
        /// <param name="parser"></param>
        public WhileStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; internal set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; internal set; }

        /// <summary>
        ///     format while statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("while");
            result.Space();
            result.Part(Condition);
            result.Space();
            result.Keyword("do");
            result.StartIndent();
            result.NewLine();
            result.Part(Statement);
            result.EndIndent();
            result.NewLine();
        }
    }
}