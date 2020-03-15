namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     intrinsic invocation result
    /// </summary>
    public interface IIntrinsicInvocationResult : IRoutineResult {

        /// <summary>
        ///     parameters
        /// </summary>
        ISignature Parameters { get; }

    }
}
