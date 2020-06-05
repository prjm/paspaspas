#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     unary intrinsic routine
    /// </summary>
    public interface IUnaryRoutine : IRoutineGroup {

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter);

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IValue ExecuteCall(IValue parameter);

        /// <summary>
        ///     check if the parameter matches
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CheckParameter(ITypeSymbol parameter);

        /// <summary>
        ///     <c>true</c> for constant routines
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        RoutineKind Kind { get; }
    }
}