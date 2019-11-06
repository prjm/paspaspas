using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     interface for constant variadic routines
    /// </summary>
    public interface IConstantVariadicRoutine : IVariadicRoutine {

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        ITypeReference ExecuteCall(Signature signature);

    }
}
