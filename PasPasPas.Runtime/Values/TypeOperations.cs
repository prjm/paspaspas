using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<int, ITypeReference> values;

        /// <summary>
        ///     create a new type operations class
        /// </summary>
        /// <param name="typeKindResolver">resolver for type kinds</param>
        public TypeOperations(ITypeRegistry typeKindResolver) {
            TypeRegistry = typeKindResolver;
            values = new LookupTable<int, ITypeReference>(MakeIndeterminedValue);
        }


        /// <summary>
        ///     nil pointer constant
        /// </summary>
        public ITypeReference Nil { get; }
            = new SpecialValue(SpecialConstantKind.Nil, KnownTypeIds.GenericPointer);

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        private ITypeReference MakeIndeterminedValue(int typeId)
            => new IndeterminedRuntimeValue(typeId, TypeRegistry.GetTypeKindOf(typeId));

        /// <summary>
        ///     create a new type reference value
        /// </summary>
        /// <param name="typeId">registered type id</param>
        /// <returns>type reference</returns>
        public ITypeReference MakeReference(int typeId)
            => values.GetValue(typeId);

        /// <summary>
        ///     create a new enumerated type value
        /// </summary>
        /// <param name="enumTypeId">enumerated type id</param>
        /// <param name="value">type value</param>
        /// <returns></returns>
        public ITypeReference MakeEnumValue(int enumTypeId, ITypeReference value)
            => new EnumeratedValue(enumTypeId, value);
    }
}
