using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor options
    /// </summary>
    public class CompilerDirectiveVisitorOptions {

        /// <summary>
        ///     compile options
        /// </summary>
        public CompileOptions CompilerOptions
            => Environment.Options.CompilerOptions;
        /// <summary>
        ///     parsing environemnt
        /// </summary>
        public ParserServices Environment { get; set; }

    }
}