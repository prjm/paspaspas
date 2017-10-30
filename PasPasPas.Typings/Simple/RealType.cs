using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type defnition
    /// </summary>
    public class RealType : TypeBase {

        /// <summary>
        ///     real type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">type name</param>
        public RealType(int withId, ScopedName withName = null) : base(withId, withName) {
        }
    }
}
