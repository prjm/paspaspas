using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     index helper for dicionaries
    /// </summary>
    /// <typeparam name="TMapType"></typeparam>
    /// <typeparam name="TTargetType"></typeparam>
    public class DictionaryIndexHelper<TMapType, TTargetType> {

        private IDictionary<TMapType, object> values;

        /// <summary>
        ///     create a new helper object
        /// </summary>
        /// <param name="dictionary"></param>
        public DictionaryIndexHelper(IDictionary<TMapType, object> dictionary)
            => values = dictionary;

        /// <summary>
        ///     get the value from the dictionary
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TTargetType this[TMapType index] {
            get {
                if (values.TryGetValue(index, out var result))
                    return (TTargetType)result;
                else
                    return default;
            }
            set => values[index] = value;
        }

        /// <summary>
        ///     reset an entry
        /// </summary>
        /// <param name="index"></param>
        public void Reset(TMapType index) => values.Remove(index);
    }
}
