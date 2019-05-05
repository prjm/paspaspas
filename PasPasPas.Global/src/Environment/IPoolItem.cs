using System;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     generic object pool item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoolItem<T> : IDisposable {

        /// <summary>
        ///     pool item
        /// </summary>
        T Item { get; }

    }
}