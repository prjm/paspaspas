using System.Collections.Generic;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     list pool helper class
    /// </summary>
    public static class ListPoolHelper {

        /// <summary>
        ///     list pool helper: add an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poolItem"></param>
        /// <param name="item"></param>
        public static void Add<T>(this IPoolItem<List<T>> poolItem, T item)
            => poolItem.Item.Add(item);

    }
}
