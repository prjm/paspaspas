﻿using System;
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
        ///     string builder pool
        /// </summary>
        public static readonly Guid StringBuilderPool
            = new Guid(new byte[] { 0xd3, 0xa9, 0xf7, 0xc2, 0x6d, 0xb6, 0xb, 0x43, 0x80, 0xbd, 0xc0, 0x61, 0x7b, 0x55, 0xaf, 0xf4 });
        /* {c2f7a9d3-b66d-430b-80bd-c0617b55aff4} */

        /// <summary>
        ///     get a string builder from the buffer pool
        /// </summary>
        /// <returns>fetched string builder</returns>
        public static ObjectPool<StringBuilder>.PoolItem FetchStringBuilder()
            => StaticEnvironment.Require<ObjectPool<StringBuilder>>(StringBuilderPool).Borrow();

    }
}
