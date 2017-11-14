﻿using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Types {

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
        /// <remarks>used only for system defined types</remarks>
        ScopedName TypeName { get; }

        /// <summary>
        ///     get the common type kind
        /// </summary>
        CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     used type registry
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

    }
}