using System;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     reference to a type definition
    /// </summary>
    internal class ReferenceToTypeDefinition : INamedTypeSymbol, IEquatable<ReferenceToTypeDefinition> {

        internal ReferenceToTypeDefinition(ITypeDefinition definition)
            => TypeDefinition = definition;

        public ITypeDefinition TypeDefinition { get; }

        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        public string Name
            => TypeDefinition.Name;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ReferenceToTypeDefinition? other)
            => other?.TypeDefinition?.Equals(TypeDefinition) ?? false;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
            => Equals(obj as ReferenceToTypeDefinition);

        /// <summary>
        ///     generate a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => TypeDefinition.GetHashCode();

        public override string ToString()
            => string.Concat(Name, "|", TypeDefinition.Name, "|", TypeDefinition.GetType().Name);

    }
}