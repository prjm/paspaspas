using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface ITypeOperations {

        /// <summary>
        ///     constant nil pointer
        /// </summary>
        ITypeReference Nil { get; }

        /// <summary>
        ///     produces a reference to a type with indeterminate compile-time value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>reference to type</returns>
        /// <param name="typeKind">type kind</param>
        ITypeReference MakeTypeInstanceReference(int typeId, CommonTypeKind typeKind);

        /// <summary>
        ///     make an enumerated type value
        /// </summary>
        /// <param name="enumTypeId">type id of the enumerated type</param>
        /// <param name="value">constant value</param>
        /// <returns>enumerated type value</returns>
        ITypeReference MakeEnumValue(int enumTypeId, ITypeReference value);

        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="routine">routine</param>
        /// <param name="routineIndex">routine index</param>
        ITypeReference MakeInvocationResult(IRoutineGroup routine, int routineIndex);

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ITypeReference MakePointerValue(int baseType, ITypeReference value);

        /// <summary>
        ///     make a type reference
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        ITypeReference MakeTypeReference(int typeId);

        /// <summary>
        ///     make a reference to the error type
        /// </summary>
        /// <returns></returns>
        ITypeReference MakeErrorTypeReference();

        /// <summary>
        ///     create a new subrange value from a simple value
        /// </summary>
        /// <param name="typeId">subrange type id</param>
        /// <param name="typeReference">wrapped value</param>
        /// <returns></returns>
        ITypeReference MakeSubrangeValue(int typeId, ITypeReference typeReference);

        /// <summary>
        ///     create a new invocation result from an intrinsic routine
        /// </summary>
        /// <param name="parameterGroup"></param>
        /// <param name="targetRoutine"></param>
        /// <returns></returns>
        IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup targetRoutine, IRoutine parameterGroup);
    }
}
