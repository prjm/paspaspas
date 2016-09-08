namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatement : SyntaxPartBase {

        /// <summary>
        ///     inherited
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        ///     name
        /// </summary>
        public NamespaceName Name { get; set; }

    }
}