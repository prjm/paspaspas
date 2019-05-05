namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     object pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPool<T> : IEnvironmentItem {

        /// <summary>
        ///     borrow an item from the pool
        /// </summary>
        /// <returns></returns>
        IPoolItem<T> Borrow();
    }
}