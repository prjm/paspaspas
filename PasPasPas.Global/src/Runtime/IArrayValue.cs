#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for array types
    /// </summary>
    public interface IArrayValue : IValue {

        /// <summary>
        ///     array base type
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     constant values
        /// </summary>
        ImmutableArray<IValue> Values { get; }
    }
}
