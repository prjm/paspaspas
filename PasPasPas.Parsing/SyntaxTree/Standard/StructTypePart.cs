namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     struct type part
    /// </summary>
    public class StructTypePart : SyntaxPartBase {

        /// <summary>
        ///     array type
        /// </summary>
        public ArrayType ArrayType { get; set; }

        /// <summary>
        ///     class type declaration
        /// </summary>
        public ClassTypeDeclaration ClassDeclaration { get; set; }

        /// <summary>
        ///     file type declaration
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        ///     set type declaration
        /// </summary>
        public SetDefinition SetType { get; set; }

    }
}