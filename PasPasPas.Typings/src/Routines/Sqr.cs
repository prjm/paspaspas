#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>sqr</c> routine
    /// </summary>
    public class Sqr : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Sqr;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Sqr;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter)
            => parameter.HasNumericType();

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {
            if (parameter is IIntegerValue)
                return Integers.Multiply(parameter, parameter);

            if (parameter is IRealNumberValue)
                return RealNumbers.Multiply(parameter, parameter);

            if (parameter is ISubrangeValue value)
                return ExecuteCall(value.WrappedValue);

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(parameter, parameter);
    }
}
