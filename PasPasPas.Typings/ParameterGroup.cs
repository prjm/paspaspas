using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     class for a paramter group
    /// </summary>
    public class ParameterGroup {

        /// <summary>
        ///     result type
        /// </summary>
        public ITypeDefinition ResultType { get; set; }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<Parameter> Parameters { get; }
            = new List<Parameter>();



    }
}