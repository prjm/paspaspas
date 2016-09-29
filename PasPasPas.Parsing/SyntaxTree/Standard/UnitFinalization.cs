namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     finalization part
    /// </summary>
    public class UnitFinalization : SyntaxPartBase {

        /// <summary>
        ///     final statements
        /// </summary>
        public StatementList Statements { get; internal set; }
    }
}