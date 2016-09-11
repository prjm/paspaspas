namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class MethodResolution : SyntaxPartBase {

        /// <summary>
        ///     kind (procedure/function)
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     resolve identifier
        /// </summary>
        public PascalIdentifier ResolveIdentifier { get; set; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public NamespaceName TypeName { get; set; }
    }
}