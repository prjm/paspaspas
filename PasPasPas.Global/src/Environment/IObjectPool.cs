#nullable disable
namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     object pool
    /// </summary>
    /// <typeparam name="T">type of the pooled object</typeparam>
    public interface IObjectPool<T> {

        /// <summary>
        ///     borrow an item from the pool
        /// </summary>
        /// <returns></returns>
        IPoolItem<T> Borrow();
    }
}