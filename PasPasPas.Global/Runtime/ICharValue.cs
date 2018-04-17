using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     widechar value (utf-16)
    /// </summary>
    public interface ICharValue : IValue {

        /// <summary>
        ///     get the wide char value
        /// </summary>
        char AsWideChar { get; }

    }
}
