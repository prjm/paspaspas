using System.Collections.Generic;
using System.Collections.Immutable;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     an interface for pooled lists
    /// </summary>
    public interface IListPools : IEnvironmentItem {

        /// <summary>
        ///     get a list pool item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IPoolItem<List<T>> GetList<T>();

        /// <summary>
        ///     get a fixed array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        ImmutableArray<T> GetFixedArray<T>(IPoolItem<List<T>> list);

    }
}
