using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     routine parameters
    /// </summary>
    public class Parameter {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeDefinition ParamType { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public string Name { get; set; }
    }
}
