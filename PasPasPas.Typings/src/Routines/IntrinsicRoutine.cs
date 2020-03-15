using System.Collections.Generic;
using PasPasPas.Globals;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     base class for intrinsic routines
    /// </summary>
    public abstract class IntrinsicRoutine : IRoutineGroup {

        /// <summary>
        ///     routine name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     runtime
        /// </summary>
        public IRuntimeValueFactory Runtime
            => TypeRegistry.Runtime;

        /// <summary>
        ///     integer values
        /// </summary>
        public IIntegerOperations Integers
            => TypeRegistry.Runtime.Integers;

        /// <summary>
        ///     chars
        /// </summary>
        public ICharOperations Chars
            => TypeRegistry.Runtime.Chars;

        /// <summary>
        ///     integer values
        /// </summary>
        public IBooleanOperations Booleans
            => TypeRegistry.Runtime.Booleans;

        /// <summary>
        ///     types
        /// </summary>
        public ITypeOperations Types
            => TypeRegistry.Runtime.Types;

        /// <summary>
        ///     strings
        /// </summary>
        public IStringOperations Strings
            => TypeRegistry.Runtime.Strings;

        /// <summary>
        ///     real number values
        /// </summary>
        public IRealNumberOperations RealNumbers
            => TypeRegistry.Runtime.RealNumbers;

        /// <summary>
        ///     parameters
        /// </summary>
        public List<IRoutine> Items { get; }
            = new List<IRoutine>();

        /// <summary>
        ///     unique routine id
        /// </summary>
        public abstract IntrinsicRoutineId RoutineId { get; }

        /// <summary>
        ///     defining type
        /// </summary>
        public ITypeDefinition DefiningType
            => TypeRegistry.SystemUnit;

        /// <summary>
        ///     symbol kind
        /// </summary>
        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.IntrinsicRoutine;

        /// <summary>
        ///     no type definition
        /// </summary>
        public ITypeDefinition TypeDefinition
            => TypeRegistry.SystemUnit.UnspecifiedType;

        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="parameter">parameter</param>
        /// <param name="returnType">return type</param>
        /// <returns></returns>
        protected IIntrinsicInvocationResult MakeResult(ITypeSymbol returnType, ITypeSymbol parameter)
            => Types.MakeInvocationResultFromIntrinsic(this, Types.MakeSignature(returnType, parameter));

        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="returnType">return type</param>
        /// <returns></returns>
        protected IIntrinsicInvocationResult MakeResult(ITypeSymbol returnType)
            => Types.MakeInvocationResultFromIntrinsic(this, Types.MakeSignature(returnType));


        /// <summary>
        ///     make an invocation result
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        protected IIntrinsicInvocationResult MakeResult(ITypeSymbol returnType, ISignature signature)
            => Types.MakeInvocationResultFromIntrinsic(this, Types.MakeSignature(returnType, signature));

        /// <summary>
        ///     make a subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue MakeSubrangeValue(ITypeDefinition typeId, IValue value)
            => Runtime?.Types?.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     create parameters
        /// </summary>
        internal virtual void CreateParameters() { }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public virtual void ResolveCall(IList<IRoutineResult> callableRoutines, ISignature signature) {
            if (this is IUnaryRoutine unaryRoutine)
                ResolveCall(unaryRoutine, callableRoutines, signature);

            if (this is IVariadicRoutine variadicRoutine)
                ResolveCall(variadicRoutine, callableRoutines, signature);
        }

        private void ResolveCall(IUnaryRoutine unaryRoutine, IList<IRoutineResult> callableRoutines, ISignature signature) {
            if (signature.Count != 1)
                return;

            var parameter = signature[0];

            if (!unaryRoutine.CheckParameter(parameter))
                return;

            var resultType = default(IRoutineResult);

            if (unaryRoutine.IsConstant && parameter.IsConstant(out var value))
                resultType = Types.MakeInvocationResultFromIntrinsic(this, unaryRoutine.ExecuteCall(value));
            else
                resultType = unaryRoutine.ResolveCall(parameter);

            callableRoutines.Add(resultType);
        }


        private void ResolveCall(IVariadicRoutine variadicRoutine, IList<IRoutineResult> callableRoutines, ISignature signature) {

            if (!variadicRoutine.CheckParameter(signature))
                return;

            var result = default(IRoutineResult);

            if (variadicRoutine.IsConstant && signature.HasConstantParameters && variadicRoutine is IConstantVariadicRoutine cvr)
                result = Types.MakeInvocationResultFromIntrinsic(this, cvr.ExecuteCall(signature));
            else
                result = variadicRoutine.ResolveCall(signature);

            callableRoutines.Add(result);
        }
    }
}
