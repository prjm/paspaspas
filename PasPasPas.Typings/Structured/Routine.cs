using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     callable routine
    /// </summary>
    public class Routine {

        /// <summary>
        ///     create a new routine
        /// </summary>
        /// <param name="name">routine name</param>
        /// <param name="kind">routine kind</param>
        public Routine(string name, ProcedureKind kind) {
            Name = name;
            Kind = kind;
        }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<ParameterGroup> Parameters { get; }
            = new List<ParameterGroup>();

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <returns></returns>
        public ParameterGroup AddParameterGroup() {
            var result = new ParameterGroup();
            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="resultType">result type</param>
        public ParameterGroup AddParameterGroup(ITypeDefinition resultType) {
            var result = new ParameterGroup {
                ResultType = resultType
            };

            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     add a parameter group
        /// </summary>
        /// <param name="firstParam">first parameter</param>
        /// <param name="resultType">result type</param>
        /// <param name="parameterName">parameter name</param>
        /// <returns></returns>
        public ParameterGroup AddParameterGroup(string parameterName, ITypeDefinition firstParam, ITypeDefinition resultType) {
            var result = new ParameterGroup {
                ResultType = resultType
            };

            result.AddParameter(parameterName).SymbolType = firstParam;

            Parameters.Add(result);
            return result;
        }
    }
}
