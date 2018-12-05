using PasPasPas.Globals.Runtime;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     unary intrinsic routine
    /// </summary>
    public interface IUnaryRoutine : IRoutine {

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ITypeReference ResolveCall(ITypeReference parameter);

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ITypeReference ExecuteCall(ITypeReference parameter);

        /// <summary>
        ///     check if the parameter matches
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CheckParameter(ITypeReference parameter);

        /// <summary>
        ///     <c>true</c> for constant routines
        /// </summary>
        bool IsConstant { get; }

    }
}