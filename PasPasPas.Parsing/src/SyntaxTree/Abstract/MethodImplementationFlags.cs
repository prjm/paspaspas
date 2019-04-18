using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method implementation flags
    /// </summary>
    [Flags]
    public enum MethodImplementationFlags {

        /// <summary>
        ///     no special flags
        /// </summary>
        None = 0,

        /// <summary>
        ///     forward declaration
        /// </summary>
        ForwardDeclaration = 1,
    }
}
