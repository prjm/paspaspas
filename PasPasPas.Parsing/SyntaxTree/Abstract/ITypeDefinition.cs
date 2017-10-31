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

        /// <summary>
        ///     get an operation of this type
        /// </summary>
        /// <param name="operationKind"></param>
        /// <returns></returns>
        IOperation GetOperation(int operationKind);
    }
}