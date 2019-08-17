﻿namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     object pool
    /// </summary>
    /// <typeparam name="T">type of the pooled object</typeparam>
    public interface IObjectPool<T> : IEnvironmentItem {

        /// <summary>
        ///     borrow an item from the pool
        /// </summary>
        /// <returns></returns>
        IPoolItem<T> Borrow();
    }
}