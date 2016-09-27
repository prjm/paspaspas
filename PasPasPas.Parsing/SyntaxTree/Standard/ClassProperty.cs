namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassProperty : SyntaxPartBase {

        /// <summary>
        ///     property access index
        /// </summary>
        public FormalParameters ArrayIndex { get; set; }

        /// <summary>
        ///     default flag (for dispinterface)
        /// </summary>
        public bool IsDefault { get; internal set; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public Expression PropertyIndex { get; set; }

        /// <summary>
        ///     property name
        /// </summary>
        public Identifier PropertyName { get; set; }

        /// <summary>
        ///     property type
        /// </summary>
        public TypeName TypeName { get; set; }

    }
}