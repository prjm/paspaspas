#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     type specification for the <code>Abs</code> routine
    /// </summary>
    public class Abs : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Abs;

        /// <summary>
        ///     check parameter type kind
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {

            if (parameter.HasNumericType())
                return true;

            if (parameter.HasSubrangeType(out var subrangeType))
                return subrangeType.SubrangeOfType.IsNumericType();

            return false;
        }

        /// <summary>
        ///     check if this routine is constant
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     <c>abs</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Abs;

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(parameter, parameter);

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {
            switch (parameter.GetBaseType()) {

                case BaseType.Integer:
                    return Integers.Abs(parameter);

                case BaseType.Real:
                    return RealNumbers.Abs(parameter);

                case BaseType.Subrange:
                    var subrangeValue = parameter as ISubrangeValue;
                    var wrappedValue = ExecuteCall(subrangeValue.WrappedValue);
                    return MakeSubrangeValue(parameter.TypeDefinition, wrappedValue);

                default:
                    return Integers.Invalid;
            }
        }
    }
}
