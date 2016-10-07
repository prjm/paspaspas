namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItem : SyntaxPartBase {

        /// <summary>
        ///     class flag
        /// </summary>
        public bool Class { get; internal set; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict visibility
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     visibility definition
        /// </summary>
        public int Visibility { get; internal set; }
    }
}