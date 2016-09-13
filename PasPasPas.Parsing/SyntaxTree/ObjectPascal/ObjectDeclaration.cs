namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     object declaration
    /// </summary>
    public class ObjectDeclaration : SyntaxPartBase {

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     object items
        /// </summary>
        public ObjectItems Items { get; set; }

    }
}