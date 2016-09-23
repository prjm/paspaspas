using System.Text;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     structur visitor options
    /// </summary>
    public class StructureVisitorOptions {

        /// <summary>
        ///     indent
        /// </summary>
        public int Indent { get; internal set; }
            = 0;

        /// <summary>
        ///     result
        /// </summary>
        public StringBuilder ResultBuilder { get; }
            = new StringBuilder();

    }
}