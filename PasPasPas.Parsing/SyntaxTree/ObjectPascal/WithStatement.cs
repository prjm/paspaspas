using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     with statement
    /// </summary>
    public class WithStatement : SyntaxPartBase {

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }

    }
}