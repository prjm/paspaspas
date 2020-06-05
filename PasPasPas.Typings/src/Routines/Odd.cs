#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     implementation for the <c>odd</c> routine
    /// </summary>
    public class Odd : IntrinsicRoutine, IUnaryRoutine {

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
            => KnownNames.Odd;

        /// <summary>
        ///     <c>odd</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Odd;

        /// <summary>
        ///     check procedure parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {
            if (parameter.GetBaseType() == BaseType.Integer)
                return true;

            if (parameter.HasSubrangeType(out var subrangeType))
                return subrangeType.SubrangeOfType.BaseType == BaseType.Integer;

            return false;
        }

        /// <summary>
        ///     execute the routine
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            var value = parameter as IIntegerValue;
            if (value == default)
                value = (parameter as ISubrangeValue)?.WrappedValue as IIntegerValue;

            if (value == default)
                return Integers.Invalid;

            if (value.SignedValue % 2 == 0)
                return Booleans.FalseValue;

            return Booleans.TrueValue;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(TypeRegistry.SystemUnit.BooleanType.Reference, parameter);
    }
}
