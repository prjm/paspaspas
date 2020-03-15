using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     result of an invocation
    /// </summary>
    public interface IInvocationResult : IRoutineResult {

        /// <summary>
        ///     get the called routine
        /// </summary>
        IRoutine RoutineIndex { get; }

    }
}
