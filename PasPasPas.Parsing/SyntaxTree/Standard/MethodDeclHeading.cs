using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclarationHeading : SyntaxPartBase {

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }

        /// <summary>
        ///     method qualifier
        /// </summary>
        public IList<MethodDeclarationName> Qualifiers { get; }
            = new List<MethodDeclarationName>();
    }
}