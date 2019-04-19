using PasPasPas.Globals.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     syntax node with type annotation
    /// </summary>
    public interface ITypedSyntaxNode {

        /// <summary>
        ///     type annotation for this syntax note
        /// </summary>
        ITypeReference TypeInfo { get; }

    }

}