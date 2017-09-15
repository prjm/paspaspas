using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PasPasPas.Infrastructure.Utils {


    /// <summary>
    /// A concrete implementation of the abstract KeyedCollection class using lambdas for the
    /// implementation.
    /// </summary>
    public class KeyedCollection2<TKey, TItem> : KeyedCollection<TKey, TItem> {
        private const string DelegateNullExceptionMessage = "Delegate passed cannot be null";
        private Func<TItem, TKey> _getKeyForItemFunction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getKeyForItemFunction"></param>
        public KeyedCollection2(Func<TItem, TKey> getKeyForItemFunction) : base()
            => _getKeyForItemFunction = getKeyForItemFunction ?? throw new ArgumentNullException(DelegateNullExceptionMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getKeyForItemDelegate"></param>
        /// <param name="comparer"></param>
        public KeyedCollection2(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey> comparer) : base(comparer)
            => _getKeyForItemFunction = getKeyForItemDelegate ?? throw new ArgumentNullException(DelegateNullExceptionMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override TKey GetKeyForItem(TItem item) => _getKeyForItemFunction(item);

        /// <summary>
        /// 
        /// </summary>
        public void SortByKeys() {
            var comparer = Comparer<TKey>.Default;
            SortByKeys(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyComparer"></param>
        public void SortByKeys(IComparer<TKey> keyComparer) {
            var comparer = new Comparer2<TItem>((x, y) => keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyComparison"></param>
        public void SortByKeys(Comparison<TKey> keyComparison) {
            var comparer = new Comparer2<TItem>((x, y) => keyComparison(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Sort() {
            var comparer = Comparer<TItem>.Default;
            Sort(comparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<TItem> comparison) {
            var newComparer = new Comparer2<TItem>((x, y) => comparison(x, y));
            Sort(newComparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<TItem> comparer) {
            if (base.Items is List<TItem> list) {
                list.Sort(comparer);
            }
        }
    }
}
