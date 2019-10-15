using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     result of an invocation
    /// </summary>
    public interface IInvocationResult : ITypeReference {

        /// <summary>
        ///     get the called routine
        /// </summary>
        IRoutine Routine { get; }

    }
}
