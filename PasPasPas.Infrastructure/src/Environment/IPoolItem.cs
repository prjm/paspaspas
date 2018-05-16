namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     interface for generic object pool items
    /// </summary>
    public interface IPoolItem {

        /// <summary>
        ///     clear the pool item
        /// </summary>
        void Clear();
    }
}