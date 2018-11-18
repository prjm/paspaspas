using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<int, ITypeReference> values;
        private readonly LookupTable<int, ITypeReference> typeRefs;

        /// <summary>
        ///     create a new type operations class
        /// </summary>
        /// <param name="types">resolver for type kinds</param>
        public TypeOperations(ITypeRegistry types) {
            TypeRegistry = types;
            values = new LookupTable<int, ITypeReference>(MakeIndeterminedValue);
            typeRefs = new LookupTable<int, ITypeReference>(MakeTypeValue);
        }

        private ITypeReference MakeTypeValue(int typeId)
            => new TypeReference(typeId) { TypeRegistry = TypeRegistry };

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
            => new IndeterminedRuntimeValue(typeId, TypeRegistry.GetTypeKindOf(typeId)) { TypeRegistry = TypeRegistry };

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

        /// <summary>
        ///     make a reference to a type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference MakeTypeReference(int typeId)
            => typeRefs.GetValue(typeId);
    }
}
