#nullable disable
using System;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     generic object pool item
    /// </summary>
    /// <typeparam name="T">type of the pooled object</typeparam>
    public interface IPoolItem<T> : IDisposable {

        /// <summary>
        ///     pool item
        /// </summary>
        T Item { get; }

    }
}