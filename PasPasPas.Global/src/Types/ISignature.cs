using System;
using System.Collections.Generic;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a signature
    /// </summary>
    public interface ISignature : IEnumerable<ITypeSymbol>, IReadOnlyList<ITypeSymbol>, IEquatable<ISignature> {

        /// <summary>
        ///     return type
        /// </summary>
        ITypeSymbol ReturnType { get; }

        /// <summary>
        ///     <c>true</c> if all parameters are constants
        /// </summary>
        bool HasConstantParameters { get; }

    }
}
