#nullable disable
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
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public interface ILookupFunction<TKey, TValue> : ILookupFunction {

        /// <summary>
        ///     access to lookup table
        /// </summary>
        new LookupTable<TKey, TValue> Table { get; }
    }
}
