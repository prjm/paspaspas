#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for a type specification
    /// </summary>
    public abstract class TypeSpecificationBase : AbstractSyntaxPartBase, ITypeSpecification, ITypedSyntaxPart {

        /// <summary>
        ///     type information
        /// </summary>
        public ITypeSymbol TypeInfo { get; set; }
    }
}