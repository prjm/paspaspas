using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>pi</c> constant value
    /// </summary>
    public class Pi : IntrinsicRoutine, IConstantVariadicRoutine {

        /// <summary>
        ///     constant function
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind
            => ProcedureKind.Function;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Pi";

        /// <summary>
        ///     <c>pi</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Pi;

        /// <summary>
        ///     check
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature)
            => signature.Length == 0;

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(Signature signature)
            => Runtime.RealNumbers.ToExtendedValue(KnownTypeIds.Extended, ExtF80.Pi);

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(Signature signature)
            => MakeTypeInstanceReference(KnownTypeIds.Extended);
    }
}
