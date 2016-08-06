using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     disp id directive
    /// </summary>
    public class DispIdDirective : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public DispIdDirective(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     disp id expression
        /// </summary>
        public Expression DispExpression { get; internal set; }

        /// <summary>
        ///     format disp id
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("dispid");
            result.Space();
            DispExpression.ToFormatter(result);
            result.Punct(";");
        }
    }
}