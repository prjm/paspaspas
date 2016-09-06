namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     property accessor
    /// </summary>
    public class ClassPropertyReadWrite : SyntaxPartBase {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceName Member { get; set; }

    }
}