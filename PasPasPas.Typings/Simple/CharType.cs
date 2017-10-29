using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     character based type
    /// </summary>
    public class CharType : TypeBase {

        /// <summary>
        ///     create a new char type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">type name</param>
        public CharType(int withId, ScopedName withName = null) : base(withId, withName) {
        }
    }
}
