namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     program header
    /// </summary>
    public class ProgramHead : SyntaxPartBase {

        /// <summary>
        ///     name of the program
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     program parameters
        /// </summary>
        public ProgramParameterList Parameters { get; set; }

    }
}