using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     get a special constant
    /// </summary>
    public enum SpecialConstantKind {

        /// <summary>
        ///     unknown constant
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     true value
        /// </summary>
        TrueValue = 1,

        /// <summary>
        ///     false value
        /// </summary>
        FalseValue = 2,

        /// <summary>
        ///     integer overflow literal
        /// </summary>
        IntegerOverflow = 3,

        /// <summary>
        ///     invalid integer
        /// </summary>
        InvalidInteger = 4,

        /// <summary>
        ///     invalid read literal
        /// </summary>
        InvalidReal = 5
    }
}
