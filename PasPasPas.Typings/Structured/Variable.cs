using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     routine parameters
    /// </summary>
    public class Variable {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeDefinition SymbolType { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     constant function parameter
        /// </summary>
        public bool ConstantParam { get; set; }
    }
}
