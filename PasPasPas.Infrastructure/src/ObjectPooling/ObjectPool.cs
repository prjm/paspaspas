using System.Collections;
using System.Collections.Concurrent;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     Sometimes, allocation a lot of small objects can produce some
    ///     memory pressure. To reduce the number of allocations, an object pool
    ///     can be used.
    ///     Objects can be borrowed from the pool (where they are allocated by demand) and
    ///     have to be returned later. This behavior can be implement with a usind
    ///     statement.
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
    ///     a generic object pool
    /// </summary>
    /// <typeparam name="TPoolItem">type of the items to pool</typeparam>
    public abstract class ObjectPool<TPoolItem> : ObjectPool where TPoolItem : new() {

        private ConcurrentQueue<PoolItem<TPoolItem>> items
            = new ConcurrentQueue<PoolItem<TPoolItem>>();

        /// <summary>
        ///     get the pool items
        /// </summary>
        protected override ICollection Items
            => items;

        /// <summary>
        ///     get one item from the pool
        /// </summary>
        /// <returns></returns>
        public PoolItem<TPoolItem> Borrow() {
            if (!items.TryDequeue(out var result))
                result = new PoolItem<TPoolItem>(new TPoolItem(), this);
            return result;
        }

        /// <summary>
        ///     get one item from the pool
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public PoolItem<TPoolItem> Borrow(out TPoolItem data) {
            var result = Borrow();
            data = result.Item;
            return result;
        }

        /// <summary>
        ///     prepare a pool item to use
        /// </summary>
        /// <param name="result">pool item to use</param>
        protected abstract void Prepare(TPoolItem result);

        /// <summary>
        ///     returns an object to the pool
        /// </summary>
        /// <param name="poolItem">item to pool</param>
        public void ReturnToPool(PoolItem<TPoolItem> poolItem) {
            Prepare(poolItem.Item);
            items.Enqueue(poolItem);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($"ObjectPool<{typeof(TPoolItem).Name}>");

    }
}
