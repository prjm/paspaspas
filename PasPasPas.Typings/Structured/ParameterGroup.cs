using System;
using System.Collections.Generic;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     class for a parameter group
    /// </summary>
    public class ParameterGroup {

        /// <summary>
        ///     result type
        /// </summary>
        public int ResultType { get; set; }

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

        /// <summary>
        ///     check if this parameter group matches a signature
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="types">type registry</param>
        /// <returns></returns>
        public bool Matches(ITypeRegistry types, Signature signature) {
            var paramCount = Parameters == null ? 0 : Parameters.Count;

            if (paramCount != signature.Length)
                return false;

            var match = true;

            for (var i = 0; Parameters != null && i < Parameters.Count; i++) {
                var parameter = Parameters[i];
                var sourceType = types.GetTypeByIdOrUndefinedType(signature[i].TypeId);
                match = match && types.GetTypeByIdOrUndefinedType(parameter.SymbolType).CanBeAssignedFrom(sourceType);

                if (!match)
                    return false;
            }

            return true;
        }
    }
}