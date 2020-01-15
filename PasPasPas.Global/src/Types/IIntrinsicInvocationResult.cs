using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     intrinsic invocation result
    /// </summary>
    public interface IIntrinsicInvocationResult : IInvocationResult {

        /// <summary>
        ///     parameters
        /// </summary>
        IRoutine Parameters { get; }

    }
}
