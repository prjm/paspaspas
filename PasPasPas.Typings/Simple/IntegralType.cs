﻿using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : TypeBase {

        /// <summary>
        ///     create a new type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="name"></param>
        public IntegralType(int withId, ScopedName name = null) : base(withId, name) {
        }
    }
}
