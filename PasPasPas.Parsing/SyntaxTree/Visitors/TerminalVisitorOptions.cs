using System.Text;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     terminal visitor options
    /// </summary>
    public class TerminalVisitorOptions {

        /// <summary>
        ///     result builder
        /// </summary>
        public StringBuilder ResultBuilder { get; }
         = new StringBuilder();
    }
}