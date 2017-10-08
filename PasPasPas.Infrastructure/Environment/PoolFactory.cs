using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     helper class for object pools
    /// </summary>
    public class PoolFactory {

        /// <summary>
        ///     get a string builder from the buffer pool
        /// </summary>
        /// <returns>fetched string builder</returns>
        public static ObjectPool<StringBuilder>.PoolItem FetchStringBuilder(StaticEnvironment environment)
            => environment.Require<ObjectPool<StringBuilder>>(StaticDependency.StringBuilderPool).Borrow();

        /// <summary>
        ///     get a generic pool item
        /// </summary>
        /// <typeparam name="T">generic pool item</typeparam>
        /// <returns>generic pool item</returns>
        public static ObjectPool<T>.PoolItem FetchGenericItem<T>(StaticEnvironment environment, int id) where T : IPoolItem, new()
            => environment.Require<ObjectPool<T>>(id).Borrow();

    }
}
