using System.Collections.Generic;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a unit type definition
    /// </summary>
    public interface IUnitType : ITypeDefinition {

        /// <summary>
        ///     registered symbols
        /// </summary>
        IEnumerable<ITypeSymbol> Symbols { get; }

        /// <summary>
        ///     provided types
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     number of registered symbol
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbol">symbol to register</param>
        void Register(ITypeSymbol symbol);
    }
}
