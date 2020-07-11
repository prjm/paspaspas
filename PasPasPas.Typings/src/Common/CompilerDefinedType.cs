using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     compiler defined type
    /// </summary>
    internal abstract class CompilerDefinedType : TypeDefinitionBase, INamedTypeSymbol, IMangledNameTypeSymbol {

        protected CompilerDefinedType(IUnitType? definingUnit) : base(definingUnit) { }

        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        /// <summary>
        ///     type name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     mangled type name
        /// </summary>
        public abstract string MangledName { get; }
    }
}
