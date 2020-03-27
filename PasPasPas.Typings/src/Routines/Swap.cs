using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>swap</c> function
    /// </summary>
    public class Swap : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     name of the function
        /// </summary>
        public override string Name
            => KnownNames.Swap;

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
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Swap;

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {
            if (parameter.GetBaseType() == BaseType.Integer)
                return true;

            if (parameter.HasSubrangeType(out var subrangeType))
                return subrangeType.BaseType == BaseType.Integer;

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter.GetBaseType() == BaseType.Integer)
                return Integers.Swap(parameter, TypeRegistry);

            if (parameter is ISubrangeValue value)
                return ExecuteCall(value.WrappedValue);

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(parameter, parameter);
    }
}
