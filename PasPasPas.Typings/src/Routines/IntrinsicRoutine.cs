using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     base class for intrinsic routines
    /// </summary>
    public abstract class IntrinsicRoutine : IRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => KnownTypeIds.IntrinsicRoutine;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     runtime
        /// </summary>
        public IRuntimeValueFactory Runtime
            => TypeRegistry?.Runtime;

        /// <summary>
        ///     integer values
        /// </summary>
        public IIntegerOperations Integers
            => TypeRegistry?.Runtime?.Integers;

        /// <summary>
        ///     real number values
        /// </summary>
        public IRealNumberOperations RealNumbers
            => TypeRegistry?.Runtime?.RealNumbers;

        /// <summary>
        ///     make a subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference MakeSubrangeValue(int typeId, ITypeReference value)
            => Runtime?.Types?.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public virtual void ResolveCall(IList<ParameterGroup> callableRoutines, Signature signature) {
            if (this is IUnaryRoutine unaryRoutine)
                ResolveCall(unaryRoutine, callableRoutines, signature);
        }

        private static void ResolveCall(IUnaryRoutine unaryRoutine, IList<ParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var parameter = signature[0];

            if (!unaryRoutine.CheckParameter(parameter))
                return;

            var result = new ParameterGroup();
            result.AddParameter("AValue").SymbolType = parameter;
            callableRoutines.Add(result);

            if (unaryRoutine.IsConstant && parameter.IsConstant())
                result.ResultType = unaryRoutine.ExecuteCall(parameter);
            else
                result.ResultType = unaryRoutine.ResolveCall(parameter);
        }


        /// <summary>
        ///     stub: make an runtime exception
        /// </summary>
        /// <returns></returns>
        protected ITypeReference RuntimeException()
            => Runtime.Types.MakeErrorTypeReference(); // ... to be changed

    }
}
