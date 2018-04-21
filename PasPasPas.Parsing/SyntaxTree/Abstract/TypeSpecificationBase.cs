using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for a type specification
    /// </summary>
    public abstract class TypeSpecificationBase : AbstractSyntaxPartBase, ITypeSpecification, ITypedSyntaxNode {

        /// <summary>
        ///     type information
        /// </summary>
        public IValue TypeInfo { get; set; }
    }
}