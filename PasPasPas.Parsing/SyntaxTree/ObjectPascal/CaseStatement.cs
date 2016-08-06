using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatement : ComposedPart<CaseItem> {


        /// <summary>
        ///     create a new case statement
        /// </summary>
        /// <param name="parser"></param>
        public CaseStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     case expression
        /// </summary>
        public Expression CaseExpression { get; internal set; }

        /// <summary>
        ///     else part
        /// </summary>
        public StatementList Else { get; internal set; }

        /// <summary>
        ///     format case statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("case").Space();
            result.Part(CaseExpression).Space();
            result.Keyword("of");
            result.StartIndent();
            result.NewLine();
            FlattenToPascal(result, x => x.NewLine());
            result.EndIndent();
            result.NewLine();
            if (Else != null) {
                result.Keyword("else");
                result.StartIndent();
                result.NewLine();
                result.Part(Else);
                result.EndIndent();
            }
            result.Keyword("end");
            result.NewLine();
        }
    }
}