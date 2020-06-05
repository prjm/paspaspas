using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic dynamic array type (<c>TArray&lt;T&gt;)</c>
    /// </summary>
    internal class GenericArrayType : ArrayType, IGenericType {

        /// <summary>
        ///     create a new generic array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType"></param>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        internal GenericArrayType(string name, IUnitType definingUnit, ITypeDefinition baseType, ITypeDefinition indexType) : base(definingUnit, indexType) {
            Name = name;
            BaseTypeDefinition = baseType;
        }

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
        public override string Name { get; }

        /// <summary>
        ///     generic array
        /// </summary>
        public override ArrayTypeKind Kind
            => ArrayTypeKind.GenericArray;

        /// <summary>
        ///     bind to a generic type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator) {
            if (typeIds.Length != 1)
                return default;

            var arrayType = typeCreator.CreateDynamicArrayType(typeIds[0], string.Empty, false);
            return arrayType;
        }
    }
}
