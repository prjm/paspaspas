using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     intrinsic routine with a variable number of parameters
    /// </summary>
    public interface IVariadicRoutine {

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        bool CheckParameter(Signature signature);

        /// <summary>
        ///     <c>true</c> if this routine is constant
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        ITypeReference ResolveCall(Signature signature);

    }
}
