namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper item
    /// </summary>
    public class ClassHelperItem : SyntaxPartBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     marker for class properties
        /// </summary>
        public bool Class { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     variable section
        /// </summary>
        public VarSection VarSection { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     constants
        /// </summary>
        public ConstSection ConstDeclaration { get; internal set; }

        /// <summary>
        ///     types
        /// </summary>
        public TypeSection TypeSection { get; internal set; }

        /// <summary>
        ///     fields
        /// </summary>
        public ClassField FieldDeclaration { get; internal set; }
    }
}