namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     except handler
    /// </summary>
    public class ExceptHandler : SyntaxPartBase {

        /// <summary>
        ///     handler type
        /// </summary>
        public NamespaceName HandlerType { get; set; }

        /// <summary>
        ///     handler name
        /// </summary>
        public PascalIdentifier Name { get; set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }

    }
}