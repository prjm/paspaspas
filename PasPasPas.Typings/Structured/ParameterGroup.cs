using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

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
        public IList<Variable> Parameters { get; private set; }

        /// <summary>
        ///     add a parameter definition
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public Variable AddParameter(string completeName) {
            if (Parameters == null)
                Parameters = new List<Variable>();

            var result = new Variable {
                Name = completeName
            };

            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     get a paramter by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Variable this[int index]
            => Parameters[index];
    }
}