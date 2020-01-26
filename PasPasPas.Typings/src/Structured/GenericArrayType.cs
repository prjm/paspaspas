using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic dynamic array type (<c>TArray&lt;T&gt;)</c>
    /// </summary>
    public class GenericArrayType : ArrayType, IGenericType {

        /// <summary>
        ///     create a new generic array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        public GenericArrayType(string name, IUnitType definingUnit, ITypeDefinition baseType) : base(definingUnit, definingUnit.TypeRegistry.SystemUnit.IntegerType) {
            Name = name;
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
        public override Reference Bind(ImmutableArray<int> typeIds) {
            if (typeIds.Length != 1)
                return default;

            var arrayType = TypeRegistry.TypeCreator.CreateDynamicArrayType(typeIds[0], false);
            return new Reference(ReferenceKind.RefToBoundGeneric, default);
        }
    }
}
