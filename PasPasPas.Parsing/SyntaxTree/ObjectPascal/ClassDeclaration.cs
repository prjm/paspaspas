namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class declaration
    /// </summary>
    public class ClassDeclaration : SyntaxPartBase {

        /// <summary>
        ///     sealed class
        /// </summary>
        public bool Abstract { get; set; }

        /// <summary>
        ///     items of a class declaration
        /// </summary>
        public ClassDeclarationItems ClassItems { get; set; }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     abstract class
        /// </summary>
        public bool Sealed { get; set; }

    }
}