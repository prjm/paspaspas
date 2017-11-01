using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     type definition
    /// </summary>
    public interface ITypeDefinition {

        /// <summary>
        ///     common type id
        /// </summary>
        int TypeId { get; }

        /// <summary>
        ///     type name (can be empty)
        /// </summary>
        ScopedName TypeName { get; }

    }
}