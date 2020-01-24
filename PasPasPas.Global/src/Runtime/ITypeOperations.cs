using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface ITypeOperations {

        /// <summary>
        ///     constant nil pointer
        /// </summary>
        IOldTypeReference Nil { get; }

        /// <summary>
        ///     produces a reference to a type with indeterminate compile-time value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>reference to type</returns>
        /// <param name="typeKind">type kind</param>
        IOldTypeReference MakeTypeInstanceReference(int typeId, CommonTypeKind typeKind);

        /// <summary>
        ///     make an enumerated type value
        /// </summary>
        /// <param name="enumTypeId">type id of the enumerated type</param>
        /// <param name="value">constant value</param>
        /// <returns>enumerated type value</returns>
        IValue MakeEnumValue(int enumTypeId, IOldTypeReference value);

        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="routine">routine</param>
        /// <param name="routineIndex">routine index</param>
        IOldTypeReference MakeInvocationResult(IRoutineGroup routine, int routineIndex);

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IOldTypeReference MakePointerValue(int baseType, IOldTypeReference value);

        /// <summary>
        ///     make a type reference
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        IOldTypeReference MakeTypeReference(int typeId);

        /// <summary>
        ///     make a reference to the error type
        /// </summary>
        /// <returns></returns>
        IOldTypeReference MakeErrorTypeReference();

        /// <summary>
        ///     create a new subrange value from a simple value
        /// </summary>
        /// <param name="typeId">subrange type id</param>
        /// <param name="typeReference">wrapped value</param>
        /// <returns></returns>
        IOldTypeReference MakeSubrangeValue(int typeId, IOldTypeReference typeReference);

        /// <summary>
        ///     create a new invocation result from an intrinsic routine
        /// </summary>
        /// <param name="parameterGroup"></param>
        /// <param name="targetRoutine"></param>
        /// <returns></returns>
        IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup targetRoutine, IRoutine parameterGroup);
    }
}
