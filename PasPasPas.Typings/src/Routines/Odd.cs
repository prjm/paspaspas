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
            => "Odd";

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
        public bool CheckParameter(ITypeReference parameter) {
            if (parameter.IsIntegral())
                return true;

            if (IsSubrangeType(parameter.TypeId, out var subrangeType))
                return subrangeType.BaseType.TypeKind.IsIntegral();

            return false;
        }

        /// <summary>
        ///     execute the routine
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (!parameter.IsIntegralValue(out var value) && !(parameter.IsSubrangeValue(out var subrangeValue) && subrangeValue.Value.IsIntegralValue(out value)))
                return RuntimeException();

            if (value.SignedValue % 2 == 0)
                return Booleans.FalseValue;

            return Booleans.TrueValue;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.BooleanType);
    }
}
