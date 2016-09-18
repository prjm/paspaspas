namespace PasPasPas.Parsing.SyntaxTree.Standard {

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
        public Identifier Name { get; set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }

    }
}