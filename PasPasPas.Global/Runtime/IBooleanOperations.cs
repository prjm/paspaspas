namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     provides logical operations and relational operations for boolean
    ///     values
    /// </summary>
    public interface IBooleanOperations : ILogicalOperations, IRelationalOperations {

        /// <summary>
        ///     value of <c>true</c>
        /// </summary>
        IValue TrueValue { get; }

        /// <summary>
        ///     value of <c>false</c>
        /// </summary>
        IValue FalseValue { get; }

        /// <summary>
        ///     invalid <c>boolean</c> value
        /// </summary>
        IValue Invalid { get; }

    }
}
