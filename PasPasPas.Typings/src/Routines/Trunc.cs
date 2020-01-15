using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>trunc</c> routine
    /// </summary>
    public class Trunc : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Trunc";

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Trunc;

        /// <summary>
        ///     check parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {
            if (parameter.IsNumerical())
                return true;

            if (IsSubrangeType(parameter.TypeId, out var value))
                return value.BaseType.TypeKind.IsNumerical();

            return false;
        }

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter is IIntegerValue integerValue)
                return parameter;

            if (parameter is ISubrangeValue subrangeValue)
                return parameter;

            if (parameter is IRealNumberValue realNumberValue)
                return RealNumbers.Trunc(realNumberValue);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.Int64Type);
    }
}
