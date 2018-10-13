using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     list pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPool<T> : ObjectPool<List<T>>, IEnvironmentItem {

        /// <summary>
        ///     pool of lists
        /// </summary>
        /// <param name="result"></param>
        protected override void Prepare(List<T> result)
            => result.Clear();
    }

    /// <summary>
    ///     manages list pools
    /// </summary>
    public class ListPools : IEnvironmentItem {

        private Dictionary<Type, object> pools
            = new Dictionary<Type, object>();

        /// <summary>
        ///     item count
        /// </summary>
        public int Count {
            get {
                var result = 0;
                foreach (var item in pools.Values)
                    result += (item as IEnvironmentItem).Count;
                return result;
            }
        }

        /// <summary>
        ///     get a list pool item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ListPool<T> GetPool<T>() where T : class {
            if (pools.TryGetValue(typeof(T), out var value))
                return value as ListPool<T>;

            var result = new ListPool<T>();
            pools.Add(typeof(T), result);
            return result;
        }

        /// <summary>
        ///     get a list pool entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PoolItem<List<T>> GetList<T>() where T : class
            => GetPool<T>().Borrow();

        /// <summary>
        ///     get an immutable array builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ImmutableArray<T>.Builder GetImmutableArrayBuilder<T>(PoolItem<List<T>> list) where T : class
            => ImmutableArray.CreateBuilder<T>(list.Item.Count);

        /// <summary>
        ///     get an immutable array builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static ImmutableArray<T>.Builder GetImmutableArrayBuilder<T>(Queue<T> queue)
            => ImmutableArray.CreateBuilder<T>(queue.Count);


    }

}
