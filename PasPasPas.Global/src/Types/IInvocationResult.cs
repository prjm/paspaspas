using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     result of an invocation
    /// </summary>
    public interface IInvocationResult : ITypeReference {

        /// <summary>
        ///     <c>true</c> if this is a compile time constant
        /// </summary>
        bool IsConstant { get; }


    }
}
