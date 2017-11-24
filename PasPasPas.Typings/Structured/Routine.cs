using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;

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
    }
}
