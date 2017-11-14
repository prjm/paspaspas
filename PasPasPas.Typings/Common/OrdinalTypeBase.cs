using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     base class for ordinal types
    /// </summary>
    public abstract class OrdinalTypeBase : TypeBase {

        /// <summary>
        ///     create a new ordinal type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="withName"></param>
        public OrdinalTypeBase(int withId, ScopedName withName = null) : base(withId, withName) {
        }
    }
}
