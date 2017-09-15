namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     interface for lookup functions
    /// </summary>
    public interface ILookupFunction {

        /// <summary>
        ///     access to lookup table
        /// </summary>
        LookupTable Table { get; }

    }

    /// <summary>
    ///     interface for lookup functions
    /// </summary>
    /// <typeparam name="Key">key type</typeparam>
    /// <typeparam name="Value">value tpe</typeparam>
    public interface ILookupFunction<Key, Value> : ILookupFunction {

        /// <summary>
        ///     access to lookup table
        /// </summary>
        new LookupTable<Key, Value> Table { get; }
    }
}
