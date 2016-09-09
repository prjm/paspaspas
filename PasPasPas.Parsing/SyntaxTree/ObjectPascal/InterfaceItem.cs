namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     interface item
    /// </summary>
    public class InterfaceItem : SyntaxPartBase {

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod Method { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty Property { get; set; }

    }
}