using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     parameter definitions
    /// </summary>
    public class ParameterDefinitions : AbstractSyntaxPart {

        /// <summary>
        ///     parameter list
        /// </summary>
        public IList<ParameterTypeDefinition> Parameters { get; }
            = new List<ParameterTypeDefinition>();

        /// <summary>
        ///     add an parameter
        /// </summary>
        /// <param name="parameter"></param>
        public void Add(ParameterTypeDefinition parameter) {
            Parameters.Add(parameter);
        }

        /// <summary>
        ///     list all parameters
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (ParameterTypeDefinition param in Parameters)
                    yield return param;
            }
        }

    }
}
