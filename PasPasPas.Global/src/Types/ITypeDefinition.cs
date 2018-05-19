﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type definition
    /// </summary>
    public interface ITypeDefinition : IRefSymbol {

        /// <summary>
        ///     get the common type kind
        /// </summary>
        CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     check if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType">type which will be assigned to this type</param>
        /// <returns><c>true</c> if the types are assignment compatible</returns>
        bool CanBeAssignedFrom(ITypeDefinition otherType);
    }
}