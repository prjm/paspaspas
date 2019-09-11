namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     interface for lookup object pools
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface ILookupByHash<TSource, TTarget> {

        /// <summary>
        ///     retrieve a value from the lookup table
        /// </summary>
        /// <param name="input"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool TryGetValue(TSource input, out TTarget target);

        /// <summary>
        ///     add a new item to the pool
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        TTarget Add(TSource input);

    }
}
