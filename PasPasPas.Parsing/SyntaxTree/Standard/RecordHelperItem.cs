namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItem : SyntaxPartBase {

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }


    }
}