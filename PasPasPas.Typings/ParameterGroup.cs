using System;
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
        public IList<Parameter> Parameters { get; private set; }

        /// <summary>
        ///     add a parameter definition
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public Parameter AddParameter(string completeName) {
            if (Parameters == null)
                Parameters = new List<Parameter>();

            var result = new Parameter {
                Name = completeName
            };

            Parameters.Add(result);
            return result;
        }
    }
}