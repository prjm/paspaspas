﻿#nullable disable
using System.Collections.Immutable;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     generic type parameter
    /// </summary>
    public interface IGenericTypeParameter : ITypeDefinition {

        /// <summary>
        ///     constraint types
        /// </summary>
        ImmutableArray<ITypeDefinition> Constraints { get; }

    }
}