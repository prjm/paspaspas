using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for a type specification
    /// </summary>
    public abstract class TypeSpecificationBase : AbstractSyntaxPartBase, ITypeSpecification, ITypedSyntaxNode {

        /// <summary>
        ///     type information
        /// </summary>
        public ITypeDefinition TypeInfo { get; set; }
    }
}