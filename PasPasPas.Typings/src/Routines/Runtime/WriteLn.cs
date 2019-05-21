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
            => "WriteLn";

        /// <summary>
        ///     dynamic routine
        /// </summary>
        public bool IsConstant
            => false;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature) {
            if (signature.Length < 1)
                return true;

            for (var i = 0; i < signature.Length; i++)
                if (!IsWriteLnType(signature[i]))
                    return false;

            return true;
        }

        private bool IsWriteLnType(ITypeReference typeReference) {
            var typeKind = typeReference.TypeKind;

            if (typeKind.IsString())
                return true;

            if (typeKind.IsChar())
                return true;

            return false;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(Signature signature) {
            var kind = TypeRegistry.GetTypeKindOf(KnownTypeIds.NoType);
            return Types.MakeInvocationResult(KnownTypeIds.NoType, kind, signature, this);
        }
    }
}
