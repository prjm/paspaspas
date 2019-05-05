using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using PasPasPas.Globals.Environment;
using PasPasPas.Infrastructure.Utils;

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
    public class ListPools : IListPools {

        private readonly Dictionary<Type, object> pools
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
        public ListPool<T> GetPool<T>() {
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
        public IPoolItem<List<T>> GetList<T>()
            => GetPool<T>().Borrow();

        /// <summary>
        ///     get an immutable array builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ImmutableArray<T>.Builder GetImmutableArrayBuilder<T>(IPoolItem<List<T>> list)
            => ImmutableArray.CreateBuilder<T>(list.Item.Count);

        /// <summary>
        ///     get an immutable array builder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static ImmutableArray<T>.Builder GetImmutableArrayBuilder<T>(Queue<T> queue)
            => ImmutableArray.CreateBuilder<T>(queue.Count);

        /// <summary>
        ///     get a fixed size array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public ImmutableArray<T> GetFixedArray<T>(IPoolItem<List<T>> list) {

            switch (list.Item.Count) {
                case 0:
                    return ImmutableArray<T>.Empty;

                case 1:
                    return ImmutableArray.Create(list.Item[0]);

                case 2:
                    return ImmutableArray.Create(list.Item[0], list.Item[1]);

                case 3:
                    return ImmutableArray.Create(list.Item[0], list.Item[1], list.Item[2]);

                case 4:
                    return ImmutableArray.Create(list.Item[0], list.Item[1], list.Item[2], list.Item[3]);

            };

            var builder = ListPools.GetImmutableArrayBuilder(list);
            for (var index = 0; index < list.Item.Count; index++)
                builder.Add(list.Item[index]);

            LogHistogram(builder);
            return builder.MoveToImmutable();
        }

        /// <summary>
        ///     get a fixed size array from a queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static ImmutableArray<T> GetFixedArray<T>(Queue<T> tokens) {
            switch (tokens.Count) {

                case 0:
                    return ImmutableArray<T>.Empty;

                case 1:
                    return ImmutableArray.Create(tokens.Dequeue());

                case 2:
                    return ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue());

                case 3:
                    return ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue());

                case 4:
                    return ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue());

            }

            var builder = ListPools.GetImmutableArrayBuilder(tokens);

            while (tokens.Count > 0)
                builder.Add(tokens.Dequeue());

            LogHistogram<T>(builder);
            return builder.MoveToImmutable();
        }

        [Conditional("DEBUG")]
        private static void LogHistogram<T>(ImmutableArray<T>.Builder builder) {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.SyntaxLists, string.Concat(typeof(T).Name, builder.Count));
        }

    }

}
