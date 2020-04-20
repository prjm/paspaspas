﻿using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     reference to a type definition
    /// </summary>
    internal class ReferenceToTypeDefinition : ITypeSymbol {

        internal ReferenceToTypeDefinition(ITypeDefinition definition)
            => TypeDefinition = definition;

        public ITypeDefinition TypeDefinition { get; }

        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;
    }
}