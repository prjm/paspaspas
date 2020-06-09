using System;
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Dynamic;
using PasPasPas.Runtime.Values.Structured.Other;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     type operations
    /// </summary>
    internal class TypeOperations : ITypeOperations {

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
        public IInvocationResult MakeInvocationResult(IRoutine routine)
            => new InvocationResult(routine);

        /// <summary>
        ///     create an invalid / error value
        /// </summary>
        /// <param name="invalidResult"></param>
        /// <returns></returns>
        public IValue MakeInvalidValue(SpecialConstantKind invalidResult)
            => new ErrorValue(provider.GetErrorType(), invalidResult);

        /// <summary>
        ///     make a new intrinsic invocation result
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult MakeInvocationResultFromIntrinsic(IRoutineGroup routine, ISignature parameters) => throw new NotImplementedException();

        /// <summary>
        ///     create an unary signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ISignature MakeSignature(ITypeSymbol returnType, ITypeSymbol parameter)
            => new Signature1(returnType, parameter);

        /// <summary>
        ///     make a variadic signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ISignature MakeSignature(ITypeSymbol returnType, IEnumerable<ITypeSymbol> signature)
            => new SignatureN(returnType, signature);

        /// <summary>
        ///     make a signature without any parameters
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public ISignature MakeSignature(ITypeSymbol returnType)
           => new Signature0(returnType);

        /// <summary>
        ///     make a signature with two parameters
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ISignature MakeSignature(ITypeSymbol resultType, ITypeSymbol left, ITypeSymbol right)
            => new Signature2(resultType, left, right);

        /// <summary>
        ///     create a signature with three parameters
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="left"></param>
        /// <param name="middle"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ISignature MakeSignature(ITypeSymbol resultType, ITypeSymbol left, ITypeSymbol middle, ITypeSymbol right)
            => new Signature3(resultType, left, middle, right);


        /// <summary>
        ///     make an operator result
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="input"></param>
        /// <param name="kind">operator kind</param>
        /// <returns></returns>
        public ITypeSymbol MakeOperatorResult(OperatorKind kind, ITypeSymbol resultType, ISignature input) {
            if (input.Count == 1)
                return new OperatorInvocationResult1(kind, resultType, input[0]);

            if (input.Count == 2)
                return new OperatorInvocationResult2(kind, resultType, input[0], input[1]);

            throw new ArgumentOutOfRangeException(nameof(input));
        }

        public ITypeSymbol MakeCastResult(ITypeSymbol fromType, ITypeDefinition toType)
            => new CastResult(fromType, toType);
    }
}
