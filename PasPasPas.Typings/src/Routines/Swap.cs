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
            => "Swap";

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
        public bool CheckParameter(IOldTypeReference parameter) {
            if (parameter.IsIntegral())
                return true;

            if (IsSubrangeType(parameter.TypeId, out var subrangeType))
                return subrangeType.BaseType.TypeKind.IsIntegral();

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ExecuteCall(IOldTypeReference parameter) {

            if (parameter.IsIntegral())
                return Integers.Swap(parameter, TypeRegistry);

            if (parameter.IsSubrangeValue(out var value))
                return ExecuteCall(value.Value);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ResolveCall(IOldTypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);
    }
}
