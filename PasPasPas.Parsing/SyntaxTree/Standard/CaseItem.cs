namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case item
    /// </summary>
    public class CaseItem : SyntaxPartBase {

        /// <summary>
        ///     case statement
        /// </summary>
        public Statement CaseStatement { get; internal set; }

    }
}