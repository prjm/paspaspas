using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     non-generic base class for an object pool
    /// </summary>
    public abstract class ObjectPool {

        /// <summary>
        ///     pool items;
        /// </summary>
        protected abstract ICollection Items { get; }

        /// <summary>
        ///     item count
        /// </summary>
        public int Count
            => Items.Count;
    }

    /// <summary>
    ///     generic object pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ObjectPool<T> : ObjectPool where T : new() {

        /// <summary>
        ///     storage for a pooled object
        /// </summary>
        public sealed class PoolItem : IDisposable {

            private readonly T t;
            private readonly ObjectPool<T> pool;

            /// <summary>
            ///     create a new object pool item
            /// </summary>
            /// <param name="t">item data</param>
            /// <param name="pool">parent pool</param>
            public PoolItem(T t, ObjectPool<T> pool) {
                this.t = t;
                this.pool = pool;
            }

            /// <summary>
            ///     return this item to the pool
            /// </summary>
            public void Dispose()
                => pool.ReturnToPool(this);

            /// <summary>
            ///     item data
            /// </summary>
            public T Data
                => t;

        }

        private ConcurrentQueue<PoolItem> items
            = new ConcurrentQueue<PoolItem>();

        /// <summary>
        ///     get the pool ites
        /// </summary>
        protected override ICollection Items
            => items;

        /// <summary>
        ///     get one item from the pool
        /// </summary>
        /// <returns></returns>
        public PoolItem Borrow() {
            if (!items.TryDequeue(out var result))
                result = new PoolItem(new T(), this);
            return result;
        }

        /// <summary>
        ///     prepare a pool item to use
        /// </summary>
        /// <param name="result">pool item to use</param>
        private void Prepare(PoolItem result) {
            switch (result.Data) {
                case StringBuilder builder:
                    builder.Clear();
                    break;

                case IPoolItem poolItem:
                    poolItem.Clear();
                    break;
            }
        }

        /// <summary>
        ///     returns an object to the pool
        /// </summary>
        /// <param name="poolItem">item to pool</param>
        private void ReturnToPool(PoolItem poolItem) {
            Prepare(poolItem);
            items.Enqueue(poolItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"ObjectPool<{typeof(T).Name}>";

    }
}
