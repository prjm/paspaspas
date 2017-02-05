namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record item
    /// </summary>
    public class RecordItem : SyntaxPartBase {

        /// <summary>
        ///     class item
        /// </summary>
        public bool Class { get; set; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSection ConstSection { get; set; }

        /// <summary>
        ///     record fields
        /// </summary>
        public RecordFieldList Fields { get; set; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict visibility
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     type
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     record variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; set; }

        /// <summary>
        ///     var section
        /// </summary>
        public VarSection VarSection { get; set; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }
    }
}