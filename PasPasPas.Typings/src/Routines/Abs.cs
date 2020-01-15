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
            => "Abs";

        /// <summary>
        ///     check parameter type kind
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
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.IsIntegral())
                return Integers.Abs(parameter);

            if (parameter.IsReal())
                return RealNumbers.Abs(parameter);

            if (parameter.IsSubrangeValue(out var value))
                return MakeSubrangeValue(parameter.TypeId, ExecuteCall(value.Value));

            return RuntimeException();
        }
    }
}
