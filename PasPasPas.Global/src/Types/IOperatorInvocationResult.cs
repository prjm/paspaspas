#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     operator invocation result
    /// </summary>
    public interface IOperatorInvocationResult : ITypeSymbol {

        /// <summary>
        ///     operator kind
        /// </summary>
        OperatorKind Kind { get; }

    }
}
