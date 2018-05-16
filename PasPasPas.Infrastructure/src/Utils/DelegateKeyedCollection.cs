using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     delegate: get the key for a collection item
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TItem">item type</typeparam>
    /// <param name="item">item</param>
    /// <returns>computed key</returns>
    public delegate TKey KeyForItem<TKey, TItem>(TItem item);

    /// <summary>
    ///     A concrete implementation of the abstract KeyedCollection class using lambdas for the
    ///     implementation.
    /// </summary>
    public class DelegateKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem> {

        private const string DelegateNullExceptionMessage = "Delegate passed cannot be null";
        private readonly KeyForItem<TKey, TItem> keyResolver;

        /// <summary>
        ///     create a new keyed collection based on a delegate
        /// </summary>
        /// <param name="resolver">key resolver</param>
        public DelegateKeyedCollection(KeyForItem<TKey, TItem> resolver) : base()
            => keyResolver = resolver ?? throw new ArgumentNullException(DelegateNullExceptionMessage);

        /// <summary>
        ///     create a new keyed collection based on a delegate
        /// </summary>
        /// <param name="resolver">key resolver</param>
        /// <param name="comparer">comparer for keys</param>
        public DelegateKeyedCollection(KeyForItem<TKey, TItem> resolver, IEqualityComparer<TKey> comparer) : base(comparer)
            => keyResolver = resolver ?? throw new ArgumentNullException(DelegateNullExceptionMessage);

        /// <summary>
        ///     resolve the key for an item
        /// </summary>
        /// <param name="item">item</param>
        /// <returns></returns>
        protected override TKey GetKeyForItem(TItem item)
            => keyResolver(item);

        /// <summary>
        ///     sort all items by keys
        /// </summary>
        public void SortByKeys()
            => SortByKeys(Comparer<TKey>.Default);

        /// <summary>
        ///     sort this collection by keys
        /// </summary>
        /// <param name="keyComparer"></param>
        public void SortByKeys(IComparer<TKey> keyComparer) {
            Sort((x, y) => keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));
        }

        /// <summary>
        ///     sort the collection by keys
        /// </summary>
        /// <param name="keyComparison"></param>
        public void SortByKeys(Comparison<TKey> keyComparison) {
            Sort((x, y) => keyComparison(GetKeyForItem(x), GetKeyForItem(y)));
        }


        /// <summary>
        ///     sort all items
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<TItem> comparison)
            => Sort((x, y) => comparison(x, y));

    }
}
