namespace PasPasPas.Parsing.SyntaxTree.Standard {

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
        public Identifier ResolveIdentifier { get; set; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public TypeName Name { get; set; }
    }
}