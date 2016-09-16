namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {


    /// <summary>
    ///     class type declarataion
    /// </summary>
    public class ClassTypeDeclaration : SyntaxPartBase {

        /// <summary>
        ///     class declaration
        /// </summary>
        public ClassDeclaration ClassDef { get; internal set; }

        /// <summary>
        ///     class helper
        /// </summary>
        public ClassHelperDef ClassHelper { get; internal set; }

        /// <summary>
        ///     class of declaration
        /// </summary>
        public ClassOfDeclaration ClassOf { get; internal set; }

        /// <summary>
        ///     interface definition
        /// </summary>
        public InterfaceDefinition InterfaceDef { get; internal set; }

        /// <summary>
        ///     object declaration
        /// </summary>
        public ObjectDeclaration ObjectDecl { get; internal set; }

        /// <summary>
        ///     record declaration
        /// </summary>
        public RecordDeclaration RecordDecl { get; internal set; }

        /// <summary>
        ///     record helper
        /// </summary>
        public RecordHelperDefinition RecordHelper { get; internal set; }

    }
}