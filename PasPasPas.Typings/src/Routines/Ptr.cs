using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>ptr</c> routine
    /// </summary>
    public class PtrRoutine : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Ptr;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     <c>ptr</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.PtrRoutine;

        /// <summary>
        ///     check
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter)
            => parameter.GetBaseType() == BaseType.Integer;

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter.GetBaseType() == BaseType.Integer) {
                var nativeInt = Integers.ToNativeInt(parameter, TypeRegistry);
                return Types.MakePointerValue(TypeRegistry.SystemUnit.GenericPointerType, nativeInt);
            }

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(TypeRegistry.SystemUnit.GenericPointerType, parameter);
    }
}
