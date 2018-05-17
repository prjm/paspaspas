using System;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     storage for a pooled object
    /// </summary>
    public sealed class PoolItem<T> : IDisposable where T : new() {

        private readonly ObjectPool<T> pool;

        /// <summary>
        ///     create a new object pool item
        /// </summary>
        /// <param name="workload">item data</param>
        /// <param name="ownerPool">parent pool</param>
        public PoolItem(T workload, ObjectPool<T> ownerPool) {
            Item = workload;
            pool = ownerPool;
        }

        /// <summary>
        ///     return this item to the pool
        /// </summary>
        public void Dispose()
            => pool.ReturnToPool(this);

        /// <summary>
        ///     item data
        /// </summary>
        public T Item { get; }

    }
}
