using System.Collections.Generic;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a unit type definition
    /// </summary>
    public interface IUnitType : ITypeDefinition, INamedTypeSymbol {

        /// <summary>
        ///     registered symbols
        /// </summary>
        IEnumerable<INamedTypeSymbol> Symbols { get; }

        /// <summary>
        ///     provided types
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbol">symbol to register</param>
        void Register(INamedTypeSymbol symbol);

        /// <summary>
        ///     try to resolve a symbol
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        bool TryToResolve(string name, out INamedTypeSymbol? reference);
    }
}
