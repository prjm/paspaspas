namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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