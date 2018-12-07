using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    /// <c>MulDivInt64</c> function
    /// </summary>
    public class MulDivInt64 : IntrinsicRoutine, IVariadicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "MulDivInt64";

        /// <summary>
        ///     intrinsic routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature) {

            if (signature.Length != 3)
                return false;

            for (var i = 0; i < signature.Length; i++) {

                if (signature[i].TypeKind != CommonTypeKind.Int64Type)
                    return false;
            }


            return true;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(Signature signature) => throw new System.NotImplementedException();

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(Signature signature) => throw new System.NotImplementedException();
    }
}
