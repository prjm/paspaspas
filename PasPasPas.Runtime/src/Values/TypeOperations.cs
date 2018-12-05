﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<(int, CommonTypeKind), ITypeReference> values;
        private readonly LookupTable<int, ITypeReference> typeRefs;

        /// <summary>
        ///     create a new type operations class
        /// </summary>
        public TypeOperations() {
            values = new LookupTable<(int, CommonTypeKind), ITypeReference>(MakeIndeterminedValue);
            typeRefs = new LookupTable<int, ITypeReference>(MakeTypeValue);
        }

        private ITypeReference MakeTypeValue(int typeId)
            => new TypeReference(typeId);

        /// <summary>
        ///     nil pointer constant
        /// </summary>
        public ITypeReference Nil { get; }
            = new SpecialValue(SpecialConstantKind.Nil, KnownTypeIds.GenericPointer);

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        private ITypeReference MakeIndeterminedValue((int typeId, CommonTypeKind typeKind) data)
            => new IndeterminedRuntimeValue(data.typeId, data.typeKind);

        /// <summary>
        ///     create a new type reference value
        /// </summary>
        /// <param name="typeId">registered type id</param>
        /// <param name="typeKind"></param>
        /// <returns>type reference</returns>
        public ITypeReference MakeTypeInstanceReference(int typeId, CommonTypeKind typeKind)
            => values.GetValue((typeId, typeKind));

        /// <summary>
        ///     create a new error type reference value
        /// </summary>
        /// <returns>type reference</returns>
        public ITypeReference MakeErrorTypeReference()
            => values.GetValue((KnownTypeIds.ErrorType, CommonTypeKind.UnknownType));


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

        /// <summary>
        ///     make a new subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public ITypeReference MakeSubrangeValue(int typeId, ITypeReference typeReference)
            => new SubrangeValue(typeId, typeReference);

    }
}
