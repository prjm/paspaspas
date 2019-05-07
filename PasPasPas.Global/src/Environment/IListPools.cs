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
        /// <typeparam name="T">list type</typeparam>
        /// <returns></returns>
        IPoolItem<List<T>> GetList<T>();

        /// <summary>
        ///     get a fixed array
        /// </summary>
        /// <typeparam name="T">list type</typeparam>
        /// <param name="list">list to convert</param>
        /// <returns>immutable array</returns>
        ImmutableArray<T> GetFixedArray<T>(IPoolItem<List<T>> list);

    }
}
