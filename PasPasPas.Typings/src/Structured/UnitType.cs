using System;
using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     unit type definition
    /// </summary>
    public class UnitType : IUnitType {

        /// <summary>
        ///     symbols
        /// </summary>
        private readonly List<ITypeSymbol>
            symbols = new List<ITypeSymbol>();

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <param name="unitName">unit name</param>
        public UnitType(string unitName, ITypeRegistry typeRegistry) {
            Name = unitName;
            TypeRegistry = typeRegistry;
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public BaseType BaseType
            => BaseType.Unit;

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public string MangledName
            => Name;

        /// <summary>
        ///     defined symbols
        /// </summary>
        public IEnumerable<ITypeSymbol> Symbols
            => symbols;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     defining unit
        /// </summary>
        public IUnitType DefiningUnit
            => this;

        /// <summary>
        ///     type size not supported
        /// </summary>
        public uint TypeSizeInBytes
            => throw new InvalidOperationException();

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => this;

        /// <summary>
        ///     type definition
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public void Register(ITypeSymbol symbol)
            => symbols.Add(symbol);
    }
}
