#nullable disable
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
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Pi;

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
        public bool CheckParameter(ISignature signature)
            => signature.Count == 0;

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IValue ExecuteCall(ISignature signature)
            => Runtime.RealNumbers.ToExtendedValue(ExtF80.Pi);

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ISignature signature)
            => MakeResult(TypeRegistry.SystemUnit.ExtendedType.Reference);
    }
}
