namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     var section
    /// </summary>
    public class VarSection : SyntaxPartBase {

        /// <summary>
        ///     section kind: var or threadvar
        /// </summary>
        public int Kind { get; set; }

    }
}
