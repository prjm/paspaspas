using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     string type definition
    /// </summary>
    public class StringType : TypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">type name</param>
        public StringType(int withId, ScopedName withName = null) : base(withId, withName) {
        }
    }
}
