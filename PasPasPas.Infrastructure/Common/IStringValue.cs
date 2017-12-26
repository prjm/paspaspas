using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : IValue {

        /// <summary>
        ///     get string valu
        /// </summary>
        string AsUnicodeString { get; }

    }
}
