namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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
        public ClassTypeDeclaration ClassDecl { get; set; }

        /// <summary>
        ///     file type declaration
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        ///     set type declaration
        /// </summary>
        public SetDef SetType { get; set; }

    }
}