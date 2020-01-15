using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<(int, CommonTypeKind), IOldTypeReference> values;
        private readonly LookupTable<int, IOldTypeReference> typeRefs;

        /// <summary>
        ///     create a new type operations class
        /// </summary>
        public TypeOperations() {
            values = new LookupTable<(int, CommonTypeKind), IOldTypeReference>(MakeIndeterminedValue);
            typeRefs = new LookupTable<int, IOldTypeReference>(MakeTypeValue);
        }

        private IOldTypeReference MakeTypeValue(int typeId)
            => new TypeReference(typeId);

        /// <summary>
        ///     nil pointer constant
        /// </summary>
        public IOldTypeReference Nil { get; }
            = new SpecialValue(SpecialConstantKind.Nil, KnownTypeIds.GenericPointer);

        private IOldTypeReference MakeIndeterminedValue((int typeId, CommonTypeKind typeKind) data)
            => new IndeterminedRuntimeValue(data.typeId, data.typeKind);

        /// <summary>
        ///     create a new type reference value
        /// </summary>
        /// <param name="typeId">registered type id</param>
        /// <param name="typeKind"></param>
        /// <returns>type reference</returns>
        public IOldTypeReference MakeTypeInstanceReference(int typeId, CommonTypeKind typeKind)
            => values.GetValue((typeId, typeKind));

        /// <summary>
        ///     create a new error type reference value
        /// </summary>
        /// <returns>type reference</returns>
        public IOldTypeReference MakeErrorTypeReference()
            => values.GetValue((KnownTypeIds.ErrorType, CommonTypeKind.UnknownType));


        /// <summary>
        ///     create a new enumerated type value
        /// </summary>
        /// <param name="enumTypeId">enumerated type id</param>
        /// <param name="value">type value</param>
        /// <returns></returns>
        public IOldTypeReference MakeEnumValue(int enumTypeId, IOldTypeReference value)
            => new EnumeratedValue(enumTypeId, value);

        /// <summary>
        ///     make a reference to a type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IOldTypeReference MakeTypeReference(int typeId)
            => typeRefs.GetValue(typeId);

        /// <summary>
        ///     make a new subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public IOldTypeReference MakeSubrangeValue(int typeId, IOldTypeReference typeReference)
            => new SubrangeValue(typeId, typeReference);

        /// <summary>
        ///     create  new pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IOldTypeReference MakePointerValue(int baseType, IOldTypeReference value)
            => new PointerValue(baseType, value);

        /// <summary>
        ///     make a new method invocation result
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="routineIndex"></param>
        public IOldTypeReference MakeInvocationResult(IRoutineGroup routine, int routineIndex)
            => new InvocationResult(routine, routineIndex);

        /// <summary>
        ///     create a new invocation result from an intrinsic routine
        /// </summary>
        /// <param name="parameterGroup"></param>
        /// <param name="targetRoutine">target routine</param>
        /// <returns></returns>
        public IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup targetRoutine, IRoutine parameterGroup)
            => new IntrinsicInvocationResult(targetRoutine, parameterGroup);
    }
}
