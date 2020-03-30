using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value factory for undefined types
    /// </summary>
    public interface ITypeOperations {

        /// <summary>
        ///     constant nil pointer
        /// </summary>
        IValue Nil { get; }

        /// <summary>
        ///     make an enumerated type value
        /// </summary>
        /// <param name="enumTypeId">type id of the enumerated type</param>
        /// <param name="value">constant value</param>
        /// <returns>enumerated type value</returns>
        IIntegerValue MakeEnumValue(ITypeDefinition enumTypeId, IIntegerValue value);

        /// <summary>
        ///     invalid value
        /// </summary>
        /// <param name="invalidResult"></param>
        /// <returns></returns>
        IValue MakeInvalidValue(SpecialConstantKind invalidResult);

        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="routine">routine</param>
        IInvocationResult MakeInvocationResult(IRoutine routine);

        /// <summary>
        ///     make a pointer value
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IValue MakePointerValue(ITypeDefinition baseType, IValue value);

        /// <summary>
        ///     create a new subrange value from a simple value
        /// </summary>
        /// <param name="typeId">subrange type id</param>
        /// <param name="wrappedValue">wrapped value</param>
        /// <returns></returns>
        IValue MakeSubrangeValue(ITypeDefinition typeId, IValue wrappedValue);

        /// <summary>
        ///     create a new invocation result from an intrinsic routine
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="routine"></param>
        /// <returns></returns>
        IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup routine, ISignature parameters);

        /// <summary>
        ///     make a unary signature
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="returnType">return type</param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol returnType, ITypeSymbol parameter);

        /// <summary>
        ///     make a variadic signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol returnType, ISignature signature);

        /// <summary>
        ///     make a signature of zero parameters
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol returnType);
    }
}
