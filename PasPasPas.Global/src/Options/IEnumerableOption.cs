using System.Collections.Generic;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     enumerable option
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEnumerableOption<T> : IEnumerable<T> {

        /// <summary>
        ///     own value
        /// </summary>
        IList<T> OwnValues { get; }
    }
}
