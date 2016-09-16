namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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
