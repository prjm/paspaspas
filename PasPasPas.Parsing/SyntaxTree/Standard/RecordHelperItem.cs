namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItem : SyntaxPartBase {

        /// <summary>
        ///     constant declaration
        /// </summary>
        public ConstSection ConstDeclaration { get; set; }

        /// <summary>
        ///     class flag
        /// </summary>
        public bool Class { get; set; }

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
        public bool Strict { get; set; }

        /// <summary>
        ///     visibility definition
        /// </summary>
        public int Visibility { get; set; }
    }
}