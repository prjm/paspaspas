using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     unit type definition
    /// </summary>
    internal class UnitType : TypeDefinitionBase, IUnitType {

        /// <summary>
        ///     symbols
        /// </summary>
        private readonly List<ITypeSymbol>
            symbols = new List<ITypeSymbol>();

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <param name="unitName">unit name</param>
        /// <param name="typeRegistry">type registry</param>
        public UnitType(string unitName, ITypeRegistry typeRegistry) : base(default)
            => Name = unitName;

        /// <summary>
        ///     type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Unit;

        /// <summary>
        ///     unit name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => Name;

        /// <summary>
        ///     defined symbols
        /// </summary>
        public IEnumerable<ITypeSymbol> Symbols
            => symbols;

        /// <summary>
        ///     type size not supported
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     unit types can not be assigned
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFromType(ITypeDefinition otherType)
            => false;

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public void Register(ITypeSymbol symbol)
            => symbols.Add(symbol);
    }
}
