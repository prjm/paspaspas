using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     maintenance interface for static cache items
    /// </summary>
    public interface IStaticCacheItem {

        /// <summary>
        ///     caption of the cache item
        /// </summary>
        string Caption { get; }
    }
}
