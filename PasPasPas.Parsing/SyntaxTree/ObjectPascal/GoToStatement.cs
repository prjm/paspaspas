using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     goto statement
    /// </summary>
    public class GoToStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new goto statement
        /// </summary>
        /// <param name="parser"></param>
        public GoToStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     break statement
        /// </summary>
        public bool Break { get; internal set; }

        /// <summary>
        ///     continue statement
        /// </summary>
        public bool Continue { get; internal set; }

        /// <summary>
        ///     exit statement
        /// </summary>
        public bool Exit { get; internal set; }

        /// <summary>
        ///     exit expression
        /// </summary>
        public Expression ExitExpression { get; internal set; }

        /// <summary>
        ///     goto label
        /// </summary>
        public Label GoToLabel { get; internal set; }

        /// <summary>
        ///     format goto statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Break) {
                result.Keyword("break");
                return;
            }
            if (Continue) {
                result.Keyword("continue");
                return;
            }
            if (GoToLabel != null) {
                result.Keyword("goto").Space();
                result.Part(GoToLabel);
                return;
            }
            if (Exit) {
                result.Keyword("exit");
                if (ExitExpression != null) {
                    result.Punct("(").Part(ExitExpression).Punct(")");
                }
            }
        }
    }
}