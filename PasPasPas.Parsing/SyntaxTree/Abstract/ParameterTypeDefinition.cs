using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterTypeDefinition : AbstractSyntaxPart, ITypeTarget {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     parameter definitions
        /// </summary>
        public IList<ParameterDefinition> Parameters { get; }
            = new List<ParameterDefinition>();

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ISyntaxPart parameter in Parameters)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }



    }
}