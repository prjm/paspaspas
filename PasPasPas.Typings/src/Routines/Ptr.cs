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
            => "Ptr";

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind
            => ProcedureKind.Function;

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
        public bool CheckParameter(ITypeReference parameter)
            => parameter.IsIntegral();

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.IsIntegral()) {
                var nativeInt = Integers.ToNativeInt(parameter, TypeRegistry);
                return Types.MakePointerValue(KnownTypeIds.UntypedPointer, nativeInt);
            }

            return RuntimeException();
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.GenericPointer);
    }
}
