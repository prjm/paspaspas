namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     syntax node with type annotation
    /// </summary>
    public interface ITypedSyntaxNode {
        /// <summary>
        ///     type info
        /// </summary>
        ITypeDefinition TypeInfo { get; }
    }
}