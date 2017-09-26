namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     a manual static cache
    /// </summary>
    public interface IManualStaticCache {

        /// <summary>
        ///     item count
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     clear the manual cache
        /// </summary>
        void Clear();
    }
}