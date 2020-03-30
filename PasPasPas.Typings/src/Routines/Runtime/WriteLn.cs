using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines.Runtime {

    /// <summary>
    ///     <c>writeln</c> routine
    /// </summary>
    public class WriteLn : IntrinsicRoutine, IVariadicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.WriteLn;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Procedure;

        /// <summary>
        ///     dynamic routine
        /// </summary>
        public bool IsConstant
            => false;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.WriteLn;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(ISignature signature) {
            if (signature.Count < 1)
                return true;

            for (var i = 0; i < signature.Count; i++)
                if (!IsWriteLnType(signature[i]))
                    return false;

            return true;
        }

        private bool IsWriteLnType(ITypeSymbol typeReference) {
            var typeKind = typeReference.TypeDefinition.BaseType;

            if (typeKind == BaseType.String)
                return true;

            if (typeKind == BaseType.Char)
                return true;

            return false;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ISignature signature)
            => MakeProcedureResult(signature);
    }
}
