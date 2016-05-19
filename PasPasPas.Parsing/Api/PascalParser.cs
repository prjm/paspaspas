using PasPasPas.Parsing.Parser;
using System.Text;

namespace PasPasPas.Api {

    /// <summary>
    ///     public interface for a pascal paser
    /// </summary>
    public static class PascalParser {

        /// <summary>
        ///     prints out the grammer
        /// </summary>
        /// <param name="result">accepted grammar</param>
        public static void PrintGrammar(StringBuilder result) {
            StandardParser.PrintGrammar(result);
        }
    }
}
