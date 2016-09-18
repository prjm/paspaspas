namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatement : SyntaxPartBase {

        /// <summary>
        ///     case expression
        /// </summary>
        public Expression CaseExpression { get; set; }

        /// <summary>
        ///     else part
        /// </summary>
        public StatementList Else { get; set; }


    }
}