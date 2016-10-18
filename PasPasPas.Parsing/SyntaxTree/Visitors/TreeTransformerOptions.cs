using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     options to transform a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformerOptions {

        /// <summary>
        ///     project root
        /// </summary>
        public ProjectRoot Project { get; }
            = new ProjectRoot();
    }
}