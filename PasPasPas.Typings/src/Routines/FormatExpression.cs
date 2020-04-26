using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     format expression helper routine
    /// </summary>
    internal class FormatExpression : IntrinsicRoutine, IVariadicRoutine {

        public override string Name
            => string.Empty;

        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.FormatExpression;

        public bool IsConstant
            => false;

        public RoutineKind Kind
            => RoutineKind.Function;

        public bool CheckParameter(ISignature signature)
            => true;

        public IIntrinsicInvocationResult ResolveCall(ISignature signature)
            => TypeRegistry.Runtime.Types.MakeInvocationResultFromIntrinsic(this, signature);

    }
}
