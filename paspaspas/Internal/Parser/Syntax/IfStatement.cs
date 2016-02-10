using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     if statement
    /// </summary>
    public class IfStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new if statement
        /// </summary>
        /// <param name="parser"></param>
        public IfStatement(IParserInformationProvider parser) : base(parser) {
        }

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; internal set; }

        /// <summary>
        ///     else part
        /// </summary>
        public Statement ElsePart { get; internal set; }

        /// <summary>
        ///     then part
        /// </summary>
        public Statement ThenPart { get; internal set; }

        /// <summary>
        ///     format the statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("if").Space();
            result.Part(Condition).Space();
            result.Keyword("then").StartIndent();
            result.NewLine().Part(ThenPart);
            result.EndIndent();
            result.NewLine();
            if (ElsePart != null) {
                result.Keyword("else").StartIndent();
                result.NewLine().Part(ThenPart);
                result.EndIndent();
                result.NewLine();
            }
        }
    }
}