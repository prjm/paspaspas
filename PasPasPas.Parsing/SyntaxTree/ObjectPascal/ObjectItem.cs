namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     object item
    /// </summary>
    public class ObjectItem : SyntaxPartBase {

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassField FieldDeclaration { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     strict modifier
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }

    }
}
