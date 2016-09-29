namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : SyntaxPartBase {

        /// <summary>
        ///     at part
        /// </summary>
        public Expression At { get; set; }

        /// <summary>
        ///     raise part
        /// </summary>
        public Expression Raise { get; set; }

    }
}