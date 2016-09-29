namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit initialization part
    /// </summary>
    public class UnitInitialization : SyntaxPartBase {

        /// <summary>
        ///     unit finalization
        /// </summary>
        public UnitFinalization Finalization { get; set; }

        /// <summary>
        ///     initialization statements
        /// </summary>
        public StatementList Statements { get; internal set; }
    }
}