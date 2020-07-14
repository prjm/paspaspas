using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     named type symbol
    /// </summary>
    internal class NamedType : INamedTypeSymbol, IMangledNameTypeSymbol {

        /// <summary>
        ///     create a new named type
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <param name="name"></param>
        /// <param name="mangledName"></param>
        public NamedType(ITypeDefinition typeDefinition, string name, string mangledName) {
            TypeDefinition = typeDefinition;
            Name = name;
            MangledName = mangledName;
        }

        /// <summary>
        ///     type name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        public string MangledName { get; }

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        public bool Equals(ITypeSymbol? typeSymbol)
            => typeSymbol is INamedTypeSymbol n &&
                KnownNames.SameIdentifier(Name, n.Name) &&
                n.TypeDefinition.Equals(TypeDefinition);
    }
}
