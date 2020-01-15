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
            => "Sqr";

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
        public bool CheckParameter(IOldTypeReference parameter)
            => parameter.IsNumerical();

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ExecuteCall(IOldTypeReference parameter) {
            if (parameter.IsIntegralValue(out var _))
                return Integers.Multiply(parameter, parameter);

            if (parameter.IsRealValue(out var _))
                return RealNumbers.Multiply(parameter, parameter);

            if (parameter.IsSubrangeValue(out var value))
                return ExecuteCall(value.Value);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ResolveCall(IOldTypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);
    }
}
