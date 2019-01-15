using System.Collections.Immutable;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     array type
    /// </summary>
    public interface IArrayType : ITypeDefinition {

        /// <summary>
        ///     index types
        /// </summary>
        ImmutableArray<int> IndexTypes { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }

    }
}
