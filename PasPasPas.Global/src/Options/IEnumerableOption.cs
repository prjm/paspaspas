using System.Collections.Generic;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     enumerable option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumerableOptionCollection<T> : IEnumerable<T> {

        /// <summary>
        ///     own value
        /// </summary>
        IList<T> OwnValues { get; }

        /// <summary>
        ///     reset option
        /// </summary>
        void ResetToDefault();

    }
}
