using PasPasPas.Parsing.Parser;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor options
    /// </summary>
    public class CompilerDirectiveVisitorOptions {

        /// <summary>
        ///     parsing environemnt
        /// </summary>
        public ParserServices Environemnt { get; set; }

    }
}