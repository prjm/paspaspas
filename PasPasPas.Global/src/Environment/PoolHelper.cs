using System.Collections.Generic;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     list pool helper class
    /// </summary>
    public static class ListPoolHelper {

        /// <summary>
        ///     list pool helper: add an item to the pooled list
        /// </summary>
        /// <typeparam name="T">list type</typeparam>
        /// <param name="poolItem">pooled list</param>
        /// <param name="item">item to add</param>
        public static T Add<T>(this IPoolItem<List<T>> poolItem, T item) {
            if (item != default)
                poolItem.Item.Add(item);
            return item;
        }
    }
}
