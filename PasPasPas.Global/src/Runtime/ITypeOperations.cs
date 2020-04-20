using System.Collections.Generic;
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
        ///     make an operator result
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="input"></param>
        /// <param name="kind">operator kind</param>
        /// <returns></returns>
        ITypeSymbol MakeOperatorResult(OperatorKind kind, ITypeSymbol resultType, ISignature input);

        /// <summary>
        ///     make a variadic signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol returnType, IEnumerable<ITypeSymbol> signature);

        /// <summary>
        ///     make a signature with two arguments
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol resultType, ITypeSymbol left, ITypeSymbol right);

        /// <summary>
        ///     make a signature of zero parameters
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        ISignature MakeSignature(ITypeSymbol returnType);

        /// <summary>
        ///     make a dynamic cast results
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        ITypeSymbol MakeCastResult(ITypeSymbol fromType, ITypeSymbol toType);
    }
}
