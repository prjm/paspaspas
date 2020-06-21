using System;
using System.Collections.Generic;
using System.Globalization;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     unit type definition
    /// </summary>
    internal class UnitType : TypeDefinitionBase, IUnitType {

        /// <summary>
        ///     symbols
        /// </summary>
        private readonly List<INamedTypeSymbol>
            symbols = new List<INamedTypeSymbol>();
        readonly ITypeRegistry typeRegistry;

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <param name="unitName">unit name</param>
        /// <param name="registry">type registry</param>
        public UnitType(string unitName, ITypeRegistry registry) : base(default) {
            Name = unitName;
            typeRegistry = registry;
        }

        public override ITypeRegistry TypeRegistry
            => typeRegistry;

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
        public IEnumerable<INamedTypeSymbol> Symbols
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

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is IUnitType u && string.Equals(u.Name, Name, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public void Register(INamedTypeSymbol symbol)
            => symbols.Add(symbol);

        public bool TryToResolve(string name, out INamedTypeSymbol? reference) {
            foreach (var item in Symbols) {
                var itemName = item.Name;

                if (item is ReferenceToTypeDefinition && item.TypeDefinition is IGenericType gt && gt.NumberOfTypeParameters > 0) {
                    itemName = string.Concat(itemName, AbstractSyntaxPartBase.GenericSeparator, gt.NumberOfTypeParameters.ToString(CultureInfo.InvariantCulture));
                }

                if (string.Equals(itemName, name, System.StringComparison.OrdinalIgnoreCase)) {
                    reference = item;
                    return true;
                }
            }

            reference = default;
            return false;
        }
    }
}
