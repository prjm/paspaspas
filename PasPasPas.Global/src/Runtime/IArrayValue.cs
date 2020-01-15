using System.Collections.Immutable;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for array types
    /// </summary>
    public interface IArrayValue : IOldTypeReference {

        /// <summary>
        ///     array base type
        /// </summary>
        int BaseType { get; }

        /// <summary>
        ///     constant values
        /// </summary>
        ImmutableArray<IOldTypeReference> Values { get; }
    }
}
