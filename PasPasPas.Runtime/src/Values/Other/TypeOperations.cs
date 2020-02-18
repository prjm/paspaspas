using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Dynamic;
using PasPasPas.Runtime.Values.Structured.Other;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        /// <summary>
        ///     create new type operation
        /// </summary>
        /// <param name="typeProvider"></param>
        public TypeOperations(ITypeRegistryProvider typeProvider) {
            provider = typeProvider;
            nil = new Lazy<IValue>(() => new NilValue(provider.GetNilType()));
        }

        /// <summary>
        ///     nil pointer constant
        /// </summary>
        public IValue Nil
            => nil.Value;

        private readonly Lazy<IValue> nil;
        private readonly ITypeRegistryProvider provider;

        /// <summary>
        ///     create a new enumerated type value
        /// </summary>
        /// <param name="enumTypeId">enumerated type id</param>
        /// <param name="value">type value</param>
        /// <returns></returns>
        public IIntegerValue MakeEnumValue(ITypeDefinition enumTypeId, IIntegerValue value)
            => new EnumeratedValue(enumTypeId, value);

        /// <summary>
        ///     make a new subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="wrappedValue"></param>
        /// <returns></returns>
        public IValue MakeSubrangeValue(ITypeDefinition typeId, IValue wrappedValue)
            => new SubrangeValue(typeId, wrappedValue);

        /// <summary>
        ///     create  new pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue MakePointerValue(ITypeDefinition baseType, IValue value)
            => new PointerValue(baseType, value);

        /// <summary>
        ///     make a new method invocation result
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="routineIndex"></param>
        public IInvocationResult MakeInvocationResult(IRoutineGroup routine, int routineIndex)
            => new InvocationResult(routine, routineIndex);

        /// <summary>
        ///     create a new invocation result from an intrinsic routine
        /// </summary>
        /// <param name="parameterGroup"></param>
        /// <param name="targetRoutine">target routine</param>
        /// <returns></returns>
        public IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup targetRoutine, IRoutine parameterGroup)
            => new IntrinsicInvocationResult(targetRoutine, parameterGroup);

        /// <summary>
        ///     create an invalid / error value
        /// </summary>
        /// <param name="invalidResult"></param>
        /// <returns></returns>
        public IValue MakeInvalidValue(SpecialConstantKind invalidResult)
            => new ErrorValue(provider.GetErrorType(), invalidResult);

    }
}
