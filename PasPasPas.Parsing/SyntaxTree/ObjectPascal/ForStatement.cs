using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     create a new for statement
    /// </summary>
    public class ForStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new for statement
        /// </summary>
        /// <param name="paser"></param>
        public ForStatement(IParserInformationProvider paser) : base(paser) { }

        /// <summary>
        ///     iteration end
        /// </summary>
        public Expression EndExpression { get; internal set; }

        /// <summary>
        ///     iteration kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     iteration start
        /// </summary>
        public Expression StartExpression { get; internal set; }

        /// <summary>
        ///     iteration statement
        /// </summary>
        public Statement Statement { get; internal set; }

        /// <summary>
        ///     iteration variable
        /// </summary>
        public DesignatorStatement Variable { get; internal set; }

        /// <summary>
        ///     format statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("for").Space();
            result.Part(Variable).Space();
            if (Kind == PascalToken.To || Kind == PascalToken.DownTo) {
                result.Operator(":=");
                result.Space().Part(StartExpression).Space();

                if (Kind == PascalToken.To) {
                    result.Keyword("to").Space();
                }
                else if (Kind == PascalToken.DownTo) {
                    result.Keyword("downto").Space();
                }
                result.Part(EndExpression).Space();
            }
            else if (Kind == PascalToken.In) {
                result.Keyword("in").Space();
                result.Part(StartExpression).Space();
            }
            result.Keyword("do").StartIndent();
            result.NewLine();
            result.Part(Statement);
            result.EndIndent();
            result.NewLine();
        }
    }
}