using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>round</c> routine
    /// </summary>
    public class Round : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Round;

        /// <summary>
        ///     <c>round</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Round;

        /// <summary>
        ///     check the parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter)
            => parameter.HasNumericType();

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter is IIntegerValue intValue)
                return intValue;

            if (parameter.HasSubrangeType(out var subrangeType) && subrangeType.SubrangeOfType.IsNumericType())
                return parameter;

            if (parameter is IRealNumberValue realValue)
                return RealNumbers.Round(realValue);

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(TypeRegistry.SystemUnit.Int64Type, parameter);
    }
}
