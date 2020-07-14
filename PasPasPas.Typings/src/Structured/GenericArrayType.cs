using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic dynamic array type (<c>TArray&lt;T&gt;)</c>
    /// </summary>
    internal class GenericArrayType : ArrayType, IGenericType, INamedTypeSymbol {

        /// <summary>
        ///     create a new generic array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType"></param>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        internal GenericArrayType(string name, IUnitType definingUnit, ITypeDefinition baseType, ITypeDefinition indexType) : base(definingUnit, indexType, baseType)
            => Name = name;

        /// <summary>
        ///     reference type / pointer type
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     number of generic type parameters
        /// </summary>
        public override int NumberOfTypeParameters
            => 1;

        /// <summary>
        ///     type name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     generic array
        /// </summary>
        public override ArrayTypeKind Kind
            => ArrayTypeKind.GenericArray;

        /// <summary>
        ///     symbol type kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        /// <summary>
        ///     bind to a generic type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <param name="typeCreator"></param>
        /// <returns></returns>
        public override ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator) {
            if (typeIds.Length != 1)
                throw new InvalidOperationException();

            var arrayType = typeCreator.CreateDynamicArrayType(typeIds[0], string.Empty, false);
            return arrayType;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ITypeSymbol? other)
            => other is GenericArrayType g &&
                KnownNames.SameIdentifier(Name, g.Name) &&
                GenericParameters.Count == 1 &&
                g.GenericParameters.Count == 1 &&
                GenericParameters[0].Equals(g.GenericParameters[0]);
    }
}
