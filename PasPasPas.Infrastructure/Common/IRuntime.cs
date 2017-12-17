namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     simple runtime definition
    /// </summary>
    public interface IRuntime {

        /// <summary>
        ///     interface for constant operations
        /// </summary>
        IConstantOperations Constants { get; }
    }
}
