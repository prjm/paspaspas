using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     case label
    /// </summary>
    public class CaseLabel : SyntaxPartBase {

        /// <summary>
        ///     create a new case label
        /// </summary>
        /// <param name="parser"></param>
        public CaseLabel(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     end expression
        /// </summary>
        public Expression EndExpression { get; internal set; }

        /// <summary>
        ///     start expression
        /// </summary>
        public Expression StartExpression { get; internal set; }

        /// <summary>
        ///     format case label
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(StartExpression);
            if (EndExpression != null) {
                result.Punct("..");
                result.Part(EndExpression);
            }
        }
    }
}